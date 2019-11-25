using System.Collections.Generic;
using System.Threading.Tasks;

using Pulumi;
using Pulumi.Gcp.Storage;
using Pulumi.Gcp.Cloudfunctions;
using Pulumi.Gcp.Cloudfunctions.Inputs;
using Pulumi.Gcp.Cloudscheduler;
using Pulumi.Gcp.Cloudscheduler.Inputs;
using Pulumi.Gcp.Pubsub;
using Pulumi.Gcp.Appengine;

class CloudFunction
{
    static Task<int> Main()
    {
        return Deployment.RunAsync(() => {
            var pubsub = new Topic("test-topic");

            // A Cloud Scheduler job cannot be created without an App Engine instance. Not sure why, 
            // but the error message is very explicit
            var appEngine = new Application("dummy-app", new ApplicationArgs { LocationId = "europe-west" });

            // Pubsub topic message must contain data otherwise you get the following error:
            // "Pubsub message must contain either non-empty data, or at least one attribute."
            var scheduler = new Job("function-trigger", new JobArgs {
                PubsubTarget = new JobPubsubTargetArgs 
                {
                    TopicName = pubsub.Id, 
                    Data = "MQ==" // This is "1" encoded in base64
                },
                TimeZone = "UTC",
                Schedule = "0 9 * * *"
            });
            
            var bucket = new Bucket("bucket");

            var bucketObject = new BucketObject("python-zip", new BucketObjectArgs
            {
                Bucket = bucket.Name,
                Source = new FileArchive("./pythonfunc")
            });

            var function = new Function("python-func", new FunctionArgs
            {
                SourceArchiveBucket = bucket.Name,
                Runtime = "python37",
                SourceArchiveObject = bucketObject.Name,
                EntryPoint = "handler",
                EventTrigger = new FunctionEventTriggerArgs 
                {
                    EventType = "providers/cloud.pubsub/eventTypes/topic.publish",
                    Resource = pubsub.Name
                },
                AvailableMemoryMb = 128
            });

            // Export the URL of the function
            return new Dictionary<string, object>
            {
                { "pythonEndpoint", function.HttpsTriggerUrl },
            };
        });
    }
}

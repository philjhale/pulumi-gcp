# pulumi-gcp
Pulumi playground

# Prerequisites

* Mac
* Pulumi `brew install pulumi`
* A Pulumi account
* [.NET Core 3.0 or later](https://dotnet.microsoft.com/download)
* gcloud CLI `brew cask install google-cloud-sdk`
* GCP project

# Configuration

[Log in to your Google account and set the project](https://www.pulumi.com/docs/intro/cloud-providers/gcp/setup/).

Set a default region.
```
pulumi config set gcp:region europe-west1
```

Deploy all the things.
```
cd [some-folder]
pulumi up
```

# Notes

* There doesn't seem to be a way of enabling GCP APIs using Pulumi. Therefore you have to do this manually


# Links

* [Getting started with Google Cloud](https://www.pulumi.com/docs/get-started/gcp/)
* [Examples](https://github.com/pulumi/examples/)
* [GCP .NET source code](https://github.com/pulumi/pulumi-gcp/tree/master/sdk/dotnet)

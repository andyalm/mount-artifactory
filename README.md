# mount-artifactory

Navigate artifactory as a virtual filesystem.

## Installation

1. Install the powershell module:
```powershell
Install-Module MountArtifactory
```

2. Add the following to your powershell profile:
```powershell
# replace with your real artifactory endpoint
$env:ARTIFACTORY_ENDPOINT="https://myartifactory.mydomain.local/artifactory"

# api key is technically optional, but you will be navigating it anonymously if you don't set one
$env:ARTIFACTORY_API_KEY="my-artifactory-api-key"

Import-Module MountArtifactory
```

3. You can now explore your artifactory repositories via the `artifactory:` PS drive.

## Usage

```
# navigate into the artifactory PS drive, which was created when you imported the MountArtifactory module, assuming you had set the `ARTIFACTORY_ENDPOINT` environment variable.
cd artifactory:

# list the artifactory repositories
dir

# list the top level directories within the myrepository repository
cd myrepository
dir

# print out the contents of the given file
gc myrepository/mydir/myfile.json
```

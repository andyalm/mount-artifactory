# mount-artifactory

Navigate artifactory as a virtual filesystem.

## Installation

1. Install the powershell module:
```powershell
Install-Module MountArtifactory
```

2. Add the following to your powershell profile. Replace the artifactory endpoint and api key with your own values:
```powershell
Import-Module MountArtifactory
New-PSDrive -Name artifactory -Provider MountArtifactory -Root '' -ArtifactoryEndpoint 'https://myartifactory.mydomain.local/artifactory' -ApiKey my-artifactory-api-key
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

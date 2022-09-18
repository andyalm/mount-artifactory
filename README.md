# mount-artifactory

Navigate artifactory as a virtual filesystem.

## Installation

1. Install the powershell module:
```
Install-Module MountArtifactory
```

2. Configure your artifactory endpoint by setting the `ARTIFACTORY_ENDPOINT` environment variable (e.g. https://myartifactory.mydomain.local/artifactory)

3. Configure your api key by setting the `ARTIFACTORY_API_KEY` environment variable

4. Import the module into your powershell session. Its recommended that you add this to your powershell profile:

```
Import-Module MountArtifactory
```

5. You can now explore your artifactory repositories via the `artifactory:` PS drive.

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

A bitbucket plugin for [SourceBrowser](https://github.com/KirillOsenkov/SourceBrowser). To add links to files in bitbucket to your SourceBrowser setup:

1. Clone this repo into the Src folder, next to the other projects.
1. Build the plugin.
1. Update the App.config of HtmlGenerator (the indexing project) to include an app setting for BitBucket:BitBucketUrl pointing at your bitbucket server, e.g.:
```
<appSettings>
        <add key="BitBucket:BitBucketUrl" value="<your url here>"/>
    </appSettings>```
1. Re-run the indexer.

Note: if you'll be making changes to SourceBrowser, you should add BitBucketGlyph to your .git/info/exclude file to prevent conflation of the repositories.
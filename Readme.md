A bitbucket plugin for [SourceBrowser](https://github.com/KirillOsenkov/SourceBrowser). To add links to files in bitbucket to your SourceBrowser setup:

Clone this repo into the Src folder, next to the other projects.

Build the plugin.

Update the App.config of HtmlGenerator (the indexing project) to include an app setting for BitBucket:BitBucketUrl pointing at your bitbucket server, e.g.:

    <appSettings>
        <add key="BitBucket:BitBucketUrl" value="<your url here>"/>
    </appSettings>

Re-run the indexer.
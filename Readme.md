This solution consists of the following major components;
- Feed Runner Console App that can be run in a docker container. This is the code responsible for reading social media stream data from providers and forwarding it to export providers.
- Azure Stream Analytics job (ASA). Peforms our analytics on incoming social media streams of data grouped by social media providers.
- Azure SQL Database. Stores pre-calculated social media metrics.
- Azure Function Http RESTful API that can run in a docker container. This has endpoints available to retreive pre-calculated social media metrics from our sql server database.

In this solution, we start off with a .Net Core 7 console app that can be run in a docker container.

This "Feed Runner" console app supports multiple Feed Providers and Multiple Feed Exporters using a feed provider factory and a feed exporter factory. Factory limits providers to enabled providers. It can
be adapted to support other social media platforms and multiple feed export targets. Currently, I have
implemented a Twitter Feed Provider and a Azure Event Hub and Azure Storage Queue Feed Export providers.

I currently only have the Azure Event Hub export provider enabled.

In the Twitter API Client Feed Provider implementation, we can set a list of matching rules to use in the streaming twitter api to filter tweets.
We can also set the Batch Size and Batching Milliseconds to use for batching incoming tweets. All events on the streaming clients are
forwarded through the implmentation so callers can get notified when the streamer starts, ends and has feed items received.

We then forward these events to receiving Export providers. I had started with with considering sending feed items to an Azure Queue and having
an Azure Function listening to the queue to perform calculations on this, but this wasn't really suited for doing dynamic queries on a stream
of data coming in. But, I kept it around since it might be a nice to have for doing additional data manipulation or gather insights in this fashion.

I decided to use Azure Event Hub as the target since it can handle bursts of millions of transactions per second depending on how much
we want to pay and scale the Event Hub accordingly. I could have used Kaifka as well.

Next, to do processing on the stream, I turned to Azure Stream Analytics. I created a Azure Stream Analytics Project in VS2022, however, it's not 
really supported in VS2022 yet. It requires VS2019 for most of the functionality you'd use locally since it relies on an older version of
Azure.Core version 1.9.0.0. VS 2022 uses a much newer version of this so we can't get the local job emulator to work with VS 2022. Keeping the project
included, we can however, open that individual project in Visual Studio Code. It supports Azure Stream Analytic projects and the extensions in there
have recently been updated. So open the main solution in VS 2022, and open the Azure Streaming Analytics project in VSCode. You can then run the job
locally picking a Cloud Source and a Cloud Target to do live processing.

The Streaming Analytics job takes 1 input stream from Azure Event Hub (our cloud job is set to only allow one stream processor at time at the price point I chose),
 but runs 4 different job processing queries on that same stream source. We output all the processing stream results from this and dump it into a SQL 
database source. This is nothing fancy but a single table that is not normalized.

One hurdle to a local developer is that ASA projects / jobs don't currently allow targeting local SQL Instances, unless you're dev machine has a officially
recognized SSL certificate mapped to your IP/DNS of your machine. The ASA job requires that the SQL Target support an encrypted connection AND it's SSL cert
is issued from an official authority. Local dev boxes, this wouldn't be feasible. So for reproing this, I did set up a cloud Azure SQL Server instance
since it works for ASA jobs.

With our stream calculations being sent to Azure SQL Server, next would be to implement something to read the resutls from there and give it to a caller.
I chose to implement an Azure Function with HTTP REST Api trigger end points. This can easily be scaled to handle may clients by default. We can then
scale the SQL instance if our load gets high enough to warrant that or we can look into partitioning etc.. But the biggest load will be on the streaming
client sending events to Azure which we've got that handled. Hundres of thousands of events can be distilled down to a few meaningful transactions every
minute or 30 seconds depending on the frequency we want so our actual SQL Server load should be quite small.

To expose this SQL Server data, I created an Entity Framework project with a seperate migrations project and included instructions on how to build an 
efbundle.exe file. This is important for CI/CD as we'll want out database build pipeline to produce an efbundle.exe file as an artifact to be used
in a release pipeline to deploy our database without having to know what migrations have already been applied. It's like the SQL Dacpac equivelent for
entity framework.

Then I created statistic data libraries with implementations for SQL Data to reterive statistics data and a statistics library to handle business logic.
We use the business logic part especially when we want to get top 10 hashtags. We don't natively capture this in the event stream, but we do get hashtag
counts. So getting hashtag counts over a period of time, it's easy to impement business logic for this to get top 10 tweet hashtags over a given time
period.

This business logic is exposed in the Azure Function API as a very light wrapper to keep the azure function slim for replacing with future technology
as needed.

=============== Setup
To set up a repro of your own solution, you'll need the following;

- Twitter API developer account with a ConsumerKey and a ConsumerSecret
- Visual Studio 2022 (with Visual Entity Framework Designer extension. Not a requirement but adding new tables code first is nice to have a visual relationship designer that generates all your extensible EF classes consistently)
- Visual Studio Code (ASA Extension needed, but should auto detect it's needed and prompt you)
- Azure Event Hub with a topic created (Azure currently does not have any local developer emulation options)
- Azure SQL Server created (Running ASA jobs locally are unable to output to a local instance of SQL Server unless your machine has a registered SSL cert from an certificate authority tied to your machine's dns. It's easier to create a free azure sql sku for testing).

Locate file appsettings.Developer.json under Feed.Runner folder
- Update line 12 and 13 with Twitter API secrets.
- Add any rules if any desired for streaming. Default is cats :). Line 19. Note, you're twitter api key sku limits how many rules you can have at a time and how many concurrent streams you can have a time (If you run Feed Runner with multiple instances, but have different rules for each for example).
- Update line 28 and 29 with the connection string from your event hub created in Azure and your hub name.
- Default Provider and Exporter is set as Twitter and AzureEventHub so no need to change anything there.
- You can play around with batching settings, line 14 and 15
- You can play around with export settings on individual sending of feed items or sending as they are batched, line 42
- There is Azure Storage Queue Client settings defined in Line 44 but only left there as an example and not used.

Locate local.settings.json under Feedstistics.Api folder
- Update line 8 with your Azure SQL Server connection string. You can get this from the azure portal but you'll have to update the connection string with your password

Locate the SocialFeed.json file under FeedstisticsTwitterStreamAnalytics folder using Visual Studio Code (Stream Input)
- You'll need to configure this to your event hub, updating Servie Bus Namespace, Hub Name, Hub Policy Name and Hub Policy Key

Locate the FeedStatisticsOutput.json file under FeedstisticsTwitterStreamAnalytics folder using Visual Studio Code (Stream Output)
-  Update the fields in there with your Server Name, Database name, Username, Password and Table Name. The Table name should be the Entity Framework project table name that was deployed.

Locate the Readme.md file under Feedstistics.Temporal.EF folder
- Instructions in this file tell how to build the entity framework project, add new migrations, and build an Entity Framework Bundle (efbundle.exe) and how to use that file to deploy a database.

================ Running

Once everything is deployed and built out, you can use Visaul Studio 2022, select start up projects on the solution level and select the Feedstistics.API project and the Feed.Runner project.
F5 to run them after. Make sure you change each indivdual project first to run as native or run as a Docker image for locally.

Next, open VS Code and open the ASA project folder in your workspace and right click on the script.asaql file and compile it then run it. You'll be prompted
for 3 different run scenarios. I used Live Cloud Input and Live Cloud Output. Then your job will start up and start processing the stream.

To fetch results, you'll notice VS2022 has an emulated Azure Function console window up, it will show you a list of routes set up. One of those routes
is a Swagger endpoint to kick off funtions. Copy that route and put it in your browser and start testing the endpoints.

================ CI/CD

I've only lightly set up CI/CD for this solution as this really depends on the target infra structure used for CI/CD. I personally would use Azure DevOps.
I did have the Azure Function REST Api and the Feed Runner Console app create Docker Files with the their build solutions so this should make it pretty
easy to get it ready for deployment to container registeries from build pipelines. Same with the efBundle.exe for the database deployment. The ASA project does produce an IaaS ARM template for deploying with Azure when you build the script in Visual Studio Code so that's handy for the release pipeline.

================ Testing
Test project and framework is intially checked in but I'll inlcude the test case in a separate project checking. Many of the scenarious that test integration
are challenging since some of the sources can't be emulated locally so we'll have to mock heavily.

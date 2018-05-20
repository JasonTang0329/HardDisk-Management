# HardDisk-Management
Provider Your Disk to Share Online(Using C#.NET)

## The idea comes from...
> ### One day, my friend ask me...  
> Jason, can you share your Nas?
>> "If I want your some files, But I don't want to via you, something can you make?"
>
>> "I think: It's OK, Maybe he just doesn't want to bother you and go via communication software or he has no idea he wants....however
>
>> "Oh-oh-oh wait on Jason, Can you make a commitment when I need to take files, I can say anyone's commit to this file??"
>
>> "Oops! It sounds great, Maybe I can do it!"

## Talk about this module how special
> 1.You and your friend can easier share when your files not with you.
>
> 2.Your friends can leave a message to let you know about the file.
>
> 3.When someone wants to download all folder, they can click download, the system will zip all of select folder and response to the user the zip file.
>
> 4.It can make short direct URL and take someone into the page.
>
> 5.It can create a virtual folder and virtual hyperlink to share someone
>
> 6.When sometimes you want to download a document, but really not sure, however, this file is you want, this system can help to preview file and decided to download or not.
>
>7.If you want to find some keywords file, you can type in a sentence, this system can segmentation the sentence and search closely files.
>
> continue developing...

## Before Download and Install, You need to prepared some environment
> * IIS: greater or equal to 7
> * .Net FrameWork: greater or equal to 4.5 
> * SQL Server: greater or equal to 2008 R2

## First, Create the database 
> * using by sql script [DataBaseScript.sql](/DataBaseScript.sql) on Sql and Execute it.

## Second, Open your IIS and setting website 
> 1. Go on your IIS and click right button of your mouse add a Web Application.
>![sample](https://puu.sh/yPtB9/a5c05ed6bb.png "sample").
> 2. Give an Application name and setting the Physical path.
>![sample](https://puu.sh/yPtT8/32a91b065c.png "sample").
> 3. Check the website working and setting correct.
>![sample](https://puu.sh/yPub4/ab0ad0eccb.png "sample").

## Thied, Change Web.config Setting  [web.config](/web.config)
> You need to change two part on this file.
>> 1. Change appSettings area reference comment out.
>
>> 2. Change connectionStrings.

## Fourth, Enjoy this module and you can share your best file and folder with anyone now!

### If you want to contact me, you can send EMAIL to [Gmail](nba032977@gmail.com), I will get back to you ASAP.

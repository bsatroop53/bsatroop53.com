# Troop 53 of Castleton NY's Website Source Code

This the the repository that contains the content and the markdown for [https://bsatroop53.com/](https://bsatroop53.com/).

## Architecture

This is a statically-generated website, powered by a forked version of [Pretzel](https://github.com/xforever1313/pretzel).  Pretzel is similar to [Jekyll](https://jekyllrb.com/), but powered by Dotnet.

The reason why this is a statically generated website is to try to make it decentralized.  The past troop websites ended up unmaintained and almost lost (had it not been for the [Wayback Machine](https://web.archive.org/)) because all the content was behind a server only 1 person had access to.  When the domain expired, the website disappeared.  With a statically generated website, backups can easily be made, and spread across multiple people.  While there is still a central point of failure if the domain expires, the content is not lost, and can easily be re-deployed elsewhere.

Another reason why is if the website was something such as Wordpress, it would involve people putting credentials on a server somewhere.  The person currently hosting the website is not comfortable with that, especially credentials of minors.

Lastly, there is nothing to hide with a statically generated website, everything is out in the open.  BSA Youth Protection Rules state that all scouting activities must be out in the open and observable by everyone.  A statically generated website exceeds that rule, as nothing is hidden behind a login.

## Creating content

The recommended way to create posts is to do so via [https://edit.bsatroop53.com/](https://edit.bsatroop53.com/).  Its too easy to make mistakes any other way.  That website will either create a .zip.bsat53 or .md.bsat53 file.  The .zip.bsat53 file should be extracted, and its file contents should go in the corresponding directory in this repo.  The md.bsat53 should be renamed to to remove the .bsat53 at the end of it, and put in the _posts folder.

## Previewing the website

* Install the .NET 8 SDK.  You can get it from [Microsoft's Website](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Clone this repo, and init the sub-modules
* If on a Unix OS, run ```./devops.sh```, while on Windows run ```.\devops.bat```.
  * If changes are made to the plugin or to pretzel after running the above command, run ```./devops.sh --target=build_all``` or ```.\devops.bat --target=build_all``` instead.
* A browser should open automatically, and you can browse the site locally.

## Deploying the website

* Install the .NET SDK and clone this repo (see previous step)
* If on a Unix OS, run ```./devops.sh --target=generate```, while on Windows run ```.\devops.bat --target=generate```.
* The contents of the ```_site``` folder should be put on the web root on the server, wherever that may be.

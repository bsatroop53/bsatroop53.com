---
layout: page
title: Mirrors
description: "Where to get a copy of this website's contents should the website go down."
comments: false
icon: "fa-solid fa-download"
exclude_from_navbar: true
---

This page explains how to download your own archival copy of the Troop 53 Website, along with how to build it and test it locally on your PC.

## Technical Information

This website is a statically generated website.  That is, all of the sites content is written in a language called [Markdown](https://en.wikipedia.org/wiki/Markdown), and then run through an [template engine](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-8.0) that transforms the site contents to [html](https://en.wikipedia.org/wiki/HTML).  The tool used to perform this is a static website generator called [Pretzel](https://github.com/xforever1313/pretzel).  Pretzel is similar to [Jekyll](https://jekyllrb.com/docs/installation/), but implemented in [Dotnet](https://en.wikipedia.org/wiki/.NET).

If that last paragraph made no sense to you, it means that the website can easily be mirrored, backed-up, and passed around by anyone.  It also means no databases or complex configuration are needed for the website to work.  The website is entirely self-contained in a couple of zip files.  It was designed this way just in case the current webmaster disappears or needs to step away from maintaining the website, the site contents are not lost to the internet like the old Troop website from the early 2000's almost was.

## Mirror Locations

While the troop website is currently hosted by Seth Hendrick, a former Troop 53 member and Eagle Scout, the source code of the website is hosted on [GitHub](https://en.wikipedia.org/wiki/GitHub) and [GitLab](https://en.wikipedia.org/wiki/GitLab).  GitHub and GitLab are platforms for software developers, and allows for multiple people to contribute to or backup software projects.

There are two parts to this website:

* [BSA Troop 53 Website Files](@Model.Site.Config["github"]) - The markdown and photos of the website itself.
* [Pretzel](https://github.com/xforever1313/pretzel) - The static website generator.

### Zip Files

The following locations are where to download the source code as a [.zip](https://en.wikipedia.org/wiki/ZIP_(file_format)) file.

* [BSA Troop 53 Website Files](@Model.Site.Config["github"])
  * [GitHub Zip File](https://github.com/bsatroop53/bsatroop53.com/archive/refs/heads/main.zip)
  * [GitLab Zip File](https://gitlab.com/xforever1313/bsatroop53-com/-/archive/main/bsatroop53-com-main.zip)
* [Pretzel](https://github.com/xforever1313/pretzel)
  * [GitHub Zip File](https://github.com/xforever1313/pretzel/archive/refs/heads/master.zip)
  * [GitLab Zip File](https://gitlab.com/xforever1313/pretzel/-/archive/master/pretzel-master.zip)

### Git

If you're familiar with [Git](https://en.wikipedia.org/wiki/Git), you can copy the website source code with a few commands:

```txt
git clone https://github.com/bsatroop53/bsatroop53.com
cd bsatroop53.com
git submodule update --init
```

## Building the Website on your PC

### Preparing the source code

You can build and test the website locally on your own computer.  To do this, download the website files and Pretzel files either by the zip links above _or_ via Git.

If you downloaded via Git, you can skip to the next step.

If you downloaded the zip files, first extract the website files.  If you enter the extracted folder, you'll notice an empty "_pretzel" folder.  Delete this folder for now.  Extract the Pretzel zip file next.  When extracting, a "pretzel-master" folder will appear.  Rename this folder to "_pretzel" and then move it and its contents into the extracted website files to replace the empty _pretzel folder that was deleted earlier.

### Installing Dotnet SDK

[Dotnet](https://en.wikipedia.org/wiki/.NET) is a runtime developed by Microsoft.  The Dotnet [SDK](https://en.wikipedia.org/wiki/Software_development_kit)(Software Development Kit) must be installed in order to build the website.

You can download the Dotnet SDK from here: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).  You'll want to download version 8.0, and click the link under the "Installers" column, rather than the "Binaries" column.  Once downloaded, go ahead and install it.

### Testing the Website

Once Dotnet is installed, you can try testing the website locally.  There are scripts in the root of the website source files you can run to do this.  If on Windows, go to your extracted website files, and double click on "devops.bat".  This will launch a command prompt.  If on MacOS or Linux, double click on "devops.sh".  This should launch a shell.

The devops script will do the following:

* Build Pretzel, the static site generator from source.
* Build the Troop 53 Site Plugins.
* Launch Pretzel, which will convert the site files from markdown to html.
* Launch your web browser, which will show you the website locally.
* Press 'q' in the command prompt or shell to stop the website from running.  After, feel free to close your web browser.

When it launches your web browser, the site you see is not the actual BSA Troop 53 website.  Instead, the web browser is showing the files from your PC instead.  No one outside of your home network can access the webpage, so don't worry about strangers getting access to files on your computer.

## Deploying the Website

Deployment is currently done by Seth Hendrick, a former Troop 53 member and Eagle Scout.  But, should he have to step away from webmaster duties, here are the instructions on how to deploy the website; at least as far as getting the website files ready.

* Open a command prompt if on Windows, or a shell on Linux or MacOS.
* Type "cd /path/to/website/files", and press enter.
  * This changes the directory of your command prompt to where the website files were extracted to.  
  * Replace "/path/to/website/files" with your actual path.
* Type ".\devops.bat --target=generate" if on Windows, or "./devops.sh --target=generate" if on MacOS or Linux and hit enter.
* The site will be build, and .html files will be put inside a "_site" folder.
* Copy the entire contents of the "_site" folder to your web root on the web hosting platform.

That last step is going to vary, depending on whatever is currently hosting the website, so unfortunately there's no generic instructions for that here.

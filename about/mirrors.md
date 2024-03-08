---
layout: page
title: Mirrors
description: "Where to get a copy of this website's contents should the website go down."
comments: false
icon: "fa-solid fa-download"
exclude_from_navbar: true
---

This website is a statically generated website.  That is, all of the sites content is written in a language called [Markdown](https://en.wikipedia.org/wiki/Markdown), and then run through an [template engine](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-8.0) that transforms the site contents to [html](https://en.wikipedia.org/wiki/HTML).  The tool used to perform this is called [Pretzel](https://github.com/xforever1313/pretzel).  Pretzel is similar to [Jekyll](https://jekyllrb.com/docs/installation/), but implemented in [Dotnet](https://en.wikipedia.org/wiki/.NET).

Because of this, it can be mirrored, backed-up, and passed around by anyone.  This is just in case the current webmaster disappears or needs to step away from maintaining the website.  Should this website go down, these are the instructions on how to bring the website back online.

## Mirror Locations

While the troop website is currently hosted by Seth Hendrick, a former Troop 53 member and Eagle Scout, the source code of the website is hosted on [GitHub](https://en.wikipedia.org/wiki/GitHub) and [GitLab](https://en.wikipedia.org/wiki/GitLab).  GitHub and GitLab are platforms for software developers, and allows for multiple people to contribute to or backup software projects.

There are three parts to this website:

* [BSA Troop 53 Website Files](@Model.Site.Config["github"]) - The markdown and photos of the website itself.
* [Pretzel](https://github.com/xforever1313/pretzel) - The static website generator.
* [StaticWebsitesCommon](https://github.com/xforever1313/StaticWebsitesCommon) - Helper methods to make a statically generated website.

### Zip Files

The following locations are where to download the source code as a [.zip](https://en.wikipedia.org/wiki/ZIP_(file_format)) file.

* [BSA Troop 53 Website Files](@Model.Site.Config["github"])
  * [GitHub Zip File](https://github.com/bsatroop53/bsatroop53.com/archive/refs/heads/main.zip)
  * [GitLab Zip File](https://gitlab.com/xforever1313/bsatroop53-com/-/archive/main/bsatroop53-com-main.zip)
* [Pretzel](https://github.com/xforever1313/pretzel)
  * [GitHub Zip File](https://github.com/xforever1313/pretzel/archive/refs/heads/master.zip)
  * [GitLab Zip File](https://gitlab.com/xforever1313/pretzel/-/archive/master/pretzel-master.zip)
* [StaticWebsitesCommon](https://github.com/xforever1313/StaticWebsitesCommon)
  * [GitHub Zip File](https://github.com/xforever1313/StaticWebsitesCommon/archive/refs/heads/main.zip)
  * [GitLab Zip File](https://gitlab.com/xforever1313/StaticWebsitesCommon/-/archive/main/StaticWebsitesCommon-main.zip)

### Git

If you're familiar with [Git](https://en.wikipedia.org/wiki/Git), you can copy the website source code with a few commands:

```txt
git clone https://github.com/bsatroop53/bsatroop53.com
cd bsatroop53.com
git submodule update --init
```

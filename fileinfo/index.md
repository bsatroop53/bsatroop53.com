---
layout: page
title: File List
description: "List of all the files reference on this website."
icon: "fa-solid fa-file"
tags: ["automation", "archive", "file", "info", "information", "ipfs", "metadata", "mirrors", "web3", "xml"]
exclude_from_navbar: true
---

This website references various external files.  For those more technically minded and wish to mass-download and backup these files, here are instructions on how to do so.

## Scouting Files

This website has a list of files that are useful for scouts or adult volunteers to reference located [here](/files/index.html).

[This XML file](/fileinfo/fileinfo.xml) contains information such as what the file is, the various editions, and the URLs in which to download them from.

* OrignalUrl - The Direct Link
* ArchiveOrgUrl - The link to an archived version via the [Wayback Machine](https://web.archive.org/).

[This XML file](/fileinfo/ipfs.xml) contains [IPFS](https://en.wikipedia.org/wiki/InterPlanetary_File_System) CIDs of each of the files.  The "name" attribute in the nodes in this file match the "filename" attribute in the [other file-info file](/fileinfo/fileinfo.xml).

For the most part, these files are _not_ hosted directly by this website, but other websites.  If you do opt to use these XML files to mass-download the files, please be a good web citizen and add some kind of rate-limiting so you don't swamp servers.

## Troop Archives

This website also has a list of files that are archives of the Troop history.  These include newspaper articles, town resolutions, etc.  You can see the full list on the archive page [here](/about/archive.html).

[This XML file](/fileinfo/archivedfileinfo.xml) contains information such as what the file is, the original source, and the URLs in which to download them from.

* DirectUrl - The Direct Link
* ArchiveOrgUrl - The link to an archived version via the [Wayback Machine](https://web.archive.org/).

[This XML file](/fileinfo/archived_files_ipfs.xml) contains [IPFS](https://en.wikipedia.org/wiki/InterPlanetary_File_System) CIDs of each of the files.  The "name" attribute in the nodes in this file match the "filename" attribute in the [other file-info file](/fileinfo/archivedfileinfo.xml).

You may notice that there is a [CSV file](/fileinfo/archivedfiledata.csv) as well.  It is recommended to not use that.  That is meant for a person to input data into, while the XML file is meant for a machine to parse.

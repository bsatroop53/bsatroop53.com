---
layout: page
title: Search
description: "Search BSA Troop 53's website"
tags: [duckduckgo, search]
icon: fa fa-search
pageindex: 1
---
@using Pretzel.Logic.Templating.Context
<iframe src="https://duckduckgo.com/search.html?site=@Model.Site.Config["urlnohttp"]&amp;prefill=Search%20with%20DuckDuckGo" style="overflow:hidden;margin:0;padding:0;height:40px;" class="ddg-search" frameborder="0"></iframe>

Search is powered by [DuckDuckGo](https://duckduckgo.com/).  The search will take you to DuckDuckGo, but limit the search to @Model.Site.Config["title"].

<noscript>
This will not work without JavaScript, you will instead search all of Duck Duck Go.
</noscript>

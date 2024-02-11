---
layout: page
title: Privacy Policy
description: "Privacy Policy"
comments: false
icon: "fas fa-lock"
exclude_from_navbar: true
---
@using Pretzel.Logic.Templating.Context
@Include( "privacy.md", Model, typeof( PageContext ) )

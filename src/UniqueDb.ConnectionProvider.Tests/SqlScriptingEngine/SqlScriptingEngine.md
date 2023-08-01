# Overview

Attempt1 was my first attempt to craft SQL that could add a column to the middle of a table.
This was useful to learn more about the problem space but ran into issues since it was fairly ad-hoc.
It seems that this technique is a dead-end and we should consider other options.

Attempt2 was moving towards a more general-purpose and powerful method for generating SQL.
It created a tree of SQL nodes, akin to a syntax tree, and then this tree could be rendered out to a text form/file.
We could create a "renderer" which could ingest this tree and output the text.
Overall thought on this approach is that this technique/approach is the ideal way to handle this.

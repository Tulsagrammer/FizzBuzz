Managed Extensibility Framework Example

A long time ago in a galaxy far, far away ... well, maybe not that long
ago: early 2015 to be a little more specific. I had some e-mail exchange
with an acquaintance regarding the "FizzBuzz" algorithm that some
organizations use as a screening test for new programmers. We had both
been exchanging clever little algorithms to see which was more cryptic
or faster or less code. I wound up collecting these algorithms along
with some scaffolding code in order to run timing tests. This became the
start of the "FizzBuzz2" project elsewhere in my Github repository.

Subsequently, FizzBuzz2 became a useful little platform for testing new
design patterns that I became exposed to. FizzBuzz2 reached its pinnacle
of success with a restructuring that allowed different output formats
(HTML, JSON) to be generated with no change to the underlying FizzBuzz
algorithm framework. But the project was still a single, static entity.
New output formats could be accommodated with minimal code change, but
the entire project still needed to be recompiled.

More recently, that acquaintance I was e-mailing a year ago presented a
small program at our local .Net Developers group that illustrated how
Reflection could be used in C# to support run-time "plug-ins". I was a
little excited to see how easy it could be to provide alternative logic
implementations without rebuilding the entire system. My immediate
reaction was to try and implement the same type of plug-ins for
FizzBuzz2. But this change would require some major restructuring of the
solution; not knowing how Git would handle new and relocated files, I
created a brand new project -- this one, FizzBuzz!

The initial version sucessfully demonstrated Reflection techniques to
assemble the system from plugins. Another refactoring has lead to the
current version which uses MEF as the driver for assembling the plugins.

FizzBuzz consists of four major components:

FizzBuzz: The overall system driver. This should be the first project
you take a look at in order to get an overview of the solution's structure.

Engine: A tiny little framework for executing FizzBuzz algorithm
plug-ins and collecting basic performance measurements. This should be
the second project you take a look at.

Algorithm Plugins: implement specific FizzBuzz algorithms as plugins to
the Engine. Sample plugins that come with this project have project
names beginning with "Algo".

Writer Plugins: generate output in various formats as plugins to
FizzBuzz. Sample writers have project names beginning with "Writer".

In addition to the core projects mentioned above, this solution contains
additional projects that illustrate other aspects of MEF.

Gotcha1: shows alternative techniques for using MEF within static methods.

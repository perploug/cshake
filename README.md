#CSHake

Proof of concept build runner, inspired by Rake and psake, runs build manifests written in csharp, through the Mono.Csharp.Evaluator.

Quick screencast here: http://www.screenr.com/ENz8

##Targets
cshake handles the structure and dependencies between build steps like nant does, so a target can depend on other targets completing first.

	ITarget defaultTarget = new Target("This is my default target", () =>
	        {
	            Console.WriteLine("This is my default delegate");
	        }
	        ).Depends(build);


##Tasks
Tasks can be performed inside targets to handle common things like calling msbuild, external .exe's and so on:

	build.Execute(() =>
                        {
                            new Exec(
                                    program: msbuildapp,
                                    workingDirectory: solutionDir,
                                    args: new string[]{ 
                                                solution,
                                                "/v:n",
                                                "/p:WarningLevel=0",
                                                "/p:ToolsVersion=4.0",
                                                "/p:Configuration=Release"})
                                    .Run(ctx);

                                    ctx.Success("Build is done");
                        })
                .Depends(test)
                .If(() =>
                {
                    //perform some logic
                    return true;
                });

##Its all csharp

So you can really do what you want, like triggering Courier Packaging tasks

	 package.Execute(() =>
            {
                new Package(
                        config: "courier.config",
                        manifest: "manifest.xml",
                        source: "clean",
                        revision: "CompleteSite"
                        ).Run(ctx);
            }
        ).Depends(test);


##Runs through Mono.csharp.Evaluator
So no compile needed, just run cshake in any directory with .cshake files in it
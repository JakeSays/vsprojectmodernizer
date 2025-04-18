﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Collections.Generic;


namespace Std.Tools.Microsoft.DotNet.Cli.CommandLine
{
	public delegate IEnumerable<string> Suggest(ParseResult parseResult);
}

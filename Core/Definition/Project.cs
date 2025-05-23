using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NuGet.Configuration;


namespace Std.Tools.Core.Definition
{
	public sealed class Project
	{
		public static readonly XNamespace XmlLegacyNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
		public XNamespace XmlNamespace => this.IsModernProject ? XNamespace.None : XmlLegacyNamespace;
		public bool IsModernProject { get; set; }

		public IReadOnlyList<AssemblyReference> AssemblyReferences { get; set; }
		public IReadOnlyList<ProjectReference> ProjectReferences { get; set; }
		public IReadOnlyList<PackageReference> PackageReferences { get; set; }
		public PackageConfiguration PackageConfiguration { get; set; }
		public AssemblyAttributes AssemblyAttributes { get; set; }
		public IReadOnlyList<XElement> PropertyGroups { get; set; }
		public IReadOnlyList<XElement> Imports { get; set; }
		public IReadOnlyList<XElement> Targets { get; set; }
		public IReadOnlyList<XElement> BuildEvents { get; set; }
		public IReadOnlyList<string> Configurations { get; set; }
		public IReadOnlyList<string> Platforms { get; set; }
		public IList<XElement> ItemGroups { get; set; }

		public XDocument ProjectDocument { get; set; }
		public string ProjectName { get; set; }

		public IList<string> TargetFrameworks { get; } = new List<string>();
		public bool? AppendTargetFrameworkToOutputPath { get; set; }
		public ApplicationType Type { get; set; }
		public FileInfo FilePath { get; set; }
		public string CodeFileExtension { get; set; } = "cs";
		public DirectoryInfo ProjectFolder => this.FilePath?.Directory;
		public Guid? ProjectGuid { get; set; }
		public string ProjectSdk { get; set; } = "Microsoft.NET.Sdk";
		public bool Valid { get; set; } = true;

		public bool HasMultipleAssemblyInfoFiles { get; set; }

		/// <summary>
		/// Files or folders that should be deleted as part of the conversion
		/// </summary>
		public IReadOnlyList<FileSystemInfo> Deletions { get; set; }

		/// <summary>
		/// The directory where NuGet stores its extracted packages for the project.
		/// In general this is the 'packages' folder within the parent solution, but
		/// it can be overridden, which is accounted for here.
		/// </summary>
		public DirectoryInfo NuGetPackagesPath
		{
			get
			{
				var projectFolder = this.ProjectFolder.FullName;

				var nuGetSettings = Settings.LoadDefaultSettings(projectFolder);
				var repositoryPathSetting = SettingsUtility.GetRepositoryPath(nuGetSettings);

				//return the explicitly set path, or if there isn't one, then use the solution's path if one was provided.
				//Otherwise assume a solution is one level above the project and therefore so is the 'packages' folder
				if (repositoryPathSetting != null)
				{
					return new DirectoryInfo(repositoryPathSetting);
				}

				if (this.Solution != null)
				{
					return this.Solution.NuGetPackagesPath;
				}

				var path = Path.GetFullPath(Path.Combine(projectFolder, "..", "packages"));

				return new DirectoryInfo(path);
			}
		}

		public FileInfo PackagesConfigFile { get; set; }

		/// <summary>
		/// The solution in which this project was found, if any.
		/// </summary>
		public Solution Solution { get; set; }

		public IReadOnlyList<XElement> AssemblyAttributeProperties { get; set; } = Array.Empty<XElement>();

		public IReadOnlyList<string> IntermediateOutputPaths { get; set; }

		private sealed class ProjectNameFilePathEqualityComparer : IEqualityComparer<Project>
		{
			public bool Equals(Project x, Project y)
			{
				if (ReferenceEquals(x, y)) return true;
				if (x is null) return false;
				if (y is null) return false;
				if (x.GetType() != y.GetType()) return false;
				return string.Equals(x.ProjectName, y.ProjectName, StringComparison.InvariantCultureIgnoreCase) &&
				       string.Equals(x.FilePath.FullName, y.FilePath.FullName,
					       StringComparison.InvariantCultureIgnoreCase);
			}

			public int GetHashCode(Project obj)
			{
				var a = obj.ProjectName != null
					? StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.ProjectName)
					: 0;
				var b = obj.FilePath != null
					? StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.FilePath.FullName)
					: 0;
				unchecked
				{
					return (a * 397) ^ b;
				}
			}
		}

		public static IEqualityComparer<Project> ProjectNameFilePathComparer { get; } = new ProjectNameFilePathEqualityComparer();
	}
}

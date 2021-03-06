﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;

namespace Bonsai.IO
{
    [DefaultProperty("Path")]
    [Description("Returns a sequence of file names and directory names that match the specified search pattern.")]
    public class EnumerateFileSystemEntries : Source<string>
    {
        public EnumerateFileSystemEntries()
        {
            Path = ".";
            SearchPattern = "*";
        }

        [Description("The path to search.")]
        [Editor("Bonsai.Design.FolderNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string Path { get; set; }

        [Description("The search string used to match against the names of files and subdirectories in the path.")]
        public string SearchPattern { get; set; }

        [Description("Specifies whether the search should include all subdirectories.")]
        public SearchOption SearchOption { get; set; }

        public override IObservable<string> Generate()
        {
            return Directory.EnumerateFileSystemEntries(Path, SearchPattern, SearchOption).ToObservable();
        }
    }
}

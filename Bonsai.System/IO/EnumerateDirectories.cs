﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.IO
{
    [DefaultProperty("Path")]
    [Description("Returns a sequence of directory names that match the specified search pattern.")]
    public class EnumerateDirectories : Source<string>
    {
        public EnumerateDirectories()
        {
            Path = ".";
            SearchPattern = "*";
        }

        [Description("The path to search.")]
        [Editor("Bonsai.Design.FolderNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        public string Path { get; set; }

        [Description("The search string used to match against the names of subdirectories in the path.")]
        public string SearchPattern { get; set; }

        [Description("Specifies whether the search should include all subdirectories.")]
        public SearchOption SearchOption { get; set; }

        public override IObservable<string> Generate()
        {
            return Directory.EnumerateDirectories(Path, SearchPattern, SearchOption).ToObservable();
        }
    }
}
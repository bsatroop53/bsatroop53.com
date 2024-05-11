//
// BSATroop53 Website Plugin - Extensions to Pretzel.
// Copyright (C) 2024 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.ObjectModel;
using System.Composition;
using System.Xml.Linq;
using dotless.Core.Parser.Tree;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace SitePlugin
{
    [Export( typeof( IBeforeProcessingTransform ) )]
    public class FileGallery : IBeforeProcessingTransform
    {
        // ---------------- Fields ----------------

        // ---------------- Constructor ----------------

        public FileGallery()
        {
        }

        static FileGallery()
        {
            var emptyDict = new Dictionary<string, IReadOnlyList<T53File>>();
            FileData = new ReadOnlyDictionary<string, IReadOnlyList<T53File>>( emptyDict );
        }

        // ---------------- Properties ----------------

        public static IReadOnlyDictionary<string, IReadOnlyList<T53File>> FileData { get; private set; }

        // ---------------- Functions ----------------

        public void Transform( SiteContext context )
        {
            Console.WriteLine( "Parsing BSA T53 file info list..." );
            Dictionary<string, string> ipfsData = ParseIpfsFile( context );
            IReadOnlyDictionary<string, IReadOnlyList<T53File>> fileData = ParseFileInfo( context, ipfsData );
            FileData = fileData;
            Console.WriteLine( "Parsing BSA T53 file info list... Done!" );
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<T53File>> ParseFileInfo(
            SiteContext context,
            Dictionary<string, string> ipfsInfo
        )
        {
            var dict = new Dictionary<string, List<T53File>>();

            var fileInfoFile = new FileInfo( Path.Combine( context.SourceFolder, "fileinfo", "fileinfo.xml" ) );

            XDocument fileInfoDoc = XDocument.Load( fileInfoFile.FullName );

            XElement? root = fileInfoDoc.Root;
            if( root is null )
            {
                Console.WriteLine( "\t- WARNING!  Can not read file info file, this page will be empty." );
            }
            else if( "Files" != root.Name.LocalName )
            {
                Console.WriteLine( "\t- WARNING!  XML root node not named 'Files'.  Is this the correct XML file?" );
            }
            else
            {
                foreach( XElement element in root.Elements() )
                {
                    if( element.Name is null )
                    {
                        Console.WriteLine( $"\t- WARNING! Found a file element whose name was null." );
                        continue;
                    }
                    else if( "File" != element.Name.LocalName )
                    {
                        Console.WriteLine( $"\t- WARNING! Found a file element whose name is not 'File', but named '{element.Name.LocalName}'." );
                        continue;
                    }

                    var file = new T53File();

                    foreach( XElement fileChild in element.Elements() )
                    {
                        if( fileChild.Name is null )
                        {
                            Console.WriteLine( $"\t- WARNING! Found a file child element whose name was null." );
                            continue;
                        }
                        else if( "DisplayName" == fileChild.Name.LocalName )
                        {
                            file = file with { DisplayName = fileChild.Value };
                        }
                        else if( "Category" == fileChild.Name.LocalName )
                        {
                            file = file with { Category = fileChild.Value };
                        }
                        else if( "Editions" == fileChild.Name.LocalName )
                        {
                            var editions = new List<T53FileEditionInfo>();

                            foreach( XElement editionsChild in fileChild.Elements() )
                            {
                                if( editionsChild.Name is null )
                                {
                                    Console.WriteLine( $"\t- WARNING! Found an editions child element whose name was null." );
                                    continue;
                                }
                                else if( "Edition" != editionsChild.Name.LocalName )
                                {
                                    Console.WriteLine( $"\t- WARNING! Found an editions child element whose name was not 'Edition', but got '{editionsChild.Name.LocalName}'." );
                                    continue;
                                }

                                var edition = new T53FileEditionInfo();

                                foreach( XAttribute editionAttr in editionsChild.Attributes() )
                                {
                                    if( editionAttr.Name is null )
                                    {
                                        Console.WriteLine( $"\t- WARNING! Found an edition attribute whose name was null." );
                                        continue;
                                    }
                                    else if( "filename" == editionAttr.Name )
                                    {
                                        string fileName = editionAttr.Value;
                                        string? ipfsCid = ipfsInfo.ContainsKey( fileName ) ? ipfsInfo[fileName] : null;
                                        edition = edition with
                                        {
                                            FileName = editionAttr.Value,
                                            IpfsCid = ipfsCid
                                        };
                                    }
                                }

                                foreach( XElement editionChild in editionsChild.Elements() )
                                {
                                    if( editionChild.Name is null )
                                    {
                                        Console.WriteLine( $"\t- WARNING! Found an edition child element attribute whose name was null." );
                                        continue;
                                    }
                                    else if( "DisplayName" == editionChild.Name.LocalName )
                                    {
                                        edition = edition with { EditionName = editionChild.Value };
                                    }
                                    else if( "OrignalUrl" == editionChild.Name.LocalName )
                                    {
                                        edition = edition with { DirectLink = new Uri( editionChild.Value ) };
                                    }
                                    else if( "ArchiveOrgUrl" == editionChild.Name.LocalName )
                                    {
                                        edition = edition with { ArchiveOrgLink = new Uri( editionChild.Value ) };
                                    }
                                }

                                editions.Add( edition );
                            }

                            file = file with { EditionList = editions.AsReadOnly() };
                        }
                    }

                    if( dict.ContainsKey( file.Category ) == false )
                    {
                        dict[file.Category] = new List<T53File>();
                    }
                    dict[file.Category].Add( file );
                }
            }

            Console.WriteLine( $"\t- Total files found: " + dict.Values.Select( l => l.Count ).Sum() );

            return new ReadOnlyDictionary<string, IReadOnlyList<T53File>>(
                new Dictionary<string, IReadOnlyList<T53File>>( dict.Select( d => new KeyValuePair<string, IReadOnlyList<T53File>>( d.Key, d.Value ) ) )
            );
        }

        private static Dictionary<string, string> ParseIpfsFile( SiteContext context )
        {
            var dict = new Dictionary<string, string>();
            
            var ipfsInfoFile = new FileInfo( Path.Combine( context.SourceFolder, "fileinfo", "ipfs.xml" ) );
            if( ipfsInfoFile.Exists == false )
            {
                Console.WriteLine( "\t- WARNING!  Can not read IPFS file, no IPFS CIDs will be included." );
                return dict;
            }

            XDocument ipfsDoc = XDocument.Load( ipfsInfoFile.FullName );

            XElement? root = ipfsDoc.Root;
            if( root is null )
            {
                Console.WriteLine( "\t- WARNING!  Can not read IPFS file (root null was null), no IPFS CIDs will be included." );
                return dict;
            }

            foreach( XElement ipfsNode in root.Elements() )
            {
                if( ipfsNode.Name is null )
                {
                    Console.WriteLine( $"\t- WARNING! Found an IPFS file element whose name was null." );
                    continue;
                }
                else if( "File".Equals( ipfsNode.Name.LocalName ) == false )
                {
                    Console.WriteLine( $"\t- WARNING! Found an IPFS file element whose name is not 'File', but named '{ipfsNode.Name.LocalName}'." );
                    continue;
                }

                string? fileId = null;
                string? cid = null;

                foreach( XAttribute attr in ipfsNode.Attributes() )
                {
                    if( "name" != attr.Name )
                    {
                        Console.WriteLine( $"\t- WARNING! Found an IPFS file attribute whose name is not 'name', but named '{attr.Name}'." );
                        continue;
                    }
                    fileId = attr.Value;
                }

                if( fileId is null )
                {
                    Console.WriteLine( $"\t- Warning, found a null IPFS file name." );
                    continue;
                }

                foreach( XElement fileNode in ipfsNode.Elements() )
                {
                    if( fileNode.Name is null )
                    {
                        Console.WriteLine( $"\t- WARNING! Found an IPFS file child element whose name was null." );
                        continue;
                    }
                    else if( "IpfsHash".Equals( fileNode.Name.LocalName ) == false )
                    {
                        Console.WriteLine( $"\t- WARNING! Found an IPFS file element whose name is not 'IpfsHash', but named '{fileNode.Name.LocalName}'." );
                        continue;
                    }

                    cid = fileNode.Value;
                }

                if( cid is null )
                {
                    Console.WriteLine( $"\t- Warning, found a null IPFS CID." );
                    continue;
                }

                dict[fileId] = cid;
            }

            Console.WriteLine( $"\t- Total IPFS CIDs found: {dict.Count}." );

            return dict;
        }
    }
}

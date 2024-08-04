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

namespace SitePlugin
{
    /// <summary>
    /// A file that is archived on https://files.bsatroop53.com
    /// </summary>
    public record class ArchivedFile
    {
        // ---------------- Fields ----------------

        private static readonly IReadOnlyList<string> defaultTags = new List<string>
        {
            "castleton ny",
            "scouting america",
            "castleton ny scouting america",
            "boy scouts",
            "bsa"
        }.AsReadOnly();

        // ---------------- Properties ----------------

        public string FileName { get; init; } = "";

        public string Category { get; init; } = "";

        public DateOnly? Date { get; init; } = null;

        public bool IsDateEstimate { get; init; } = false;

        public string OriginalSource { get; init; } = "";

        public string PageNumber { get; init; } = "";

        public string AuthorFirstName { get; init; } = "";

        public string AuthorLastName { get; init; } = "";

        public string Database { get; init; } = "";

        public bool Troop53Mentioned { get; init; } = false;

        public bool Troop253Mentioned { get; init; } = false;

        public bool Pack253Mentioned { get; init; } = false;

        public bool Crew153Mentioned { get; init; } = false;

        public string? ArchiveOrgLink { get; init; } = null;

        public string Title { get; init; } = "";

        public string[] Tags { get; init; } = Array.Empty<string>();

        public string? IpfsCid { get; init; }

        // ---------------- Functions ----------------

        public bool IsAuthorSpecified()
        {
            return
                ( string.IsNullOrWhiteSpace( this.AuthorFirstName ) == false ) &&
                ( string.IsNullOrWhiteSpace( this.AuthorLastName ) == false );
        }

        public IEnumerable<string> GetAllTags()
        {
            var list = new List<string>();
            list.AddRange( this.Tags );
            list.AddRange( defaultTags );

            if( this.Troop253Mentioned )
            {
                list.Add( "troop 253" );
            }

            if( this.Troop53Mentioned )
            {
                list.Add( "troop 53" );
            }

            if( this.Pack253Mentioned )
            {
                list.Add( "pack 253" );
            }

            if( this.Crew153Mentioned )
            {
                list.Add( "crew 153" );
            }

            if( string.IsNullOrWhiteSpace( this.OriginalSource ) == false )
            {
                list.Add( this.OriginalSource.ToLower() );
            }

            if( string.IsNullOrWhiteSpace( this.Category ) == false )
            {
                // Make singular.
                list.Add( this.Category.ToLower().TrimEnd( 's' ) );
            }

            return list;
        }

        public Uri GetDirectLink()
        {
            if( this.Date is null )
            {
                throw new InvalidOperationException( $"Archived file '{this.FileName}' has no year." );
            }

            return new Uri( $"https://files.bsatroop53.com/{Category}/{this.Date.Value.Year}/{this.FileName}" );
        }
    }
}

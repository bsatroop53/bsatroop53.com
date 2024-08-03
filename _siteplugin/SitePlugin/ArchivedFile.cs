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
        // ---------------- Properties ----------------

        public string FileName { get; init; } = "";

        public string Category { get; init; } = "";

        public DateTime? Date { get; init; } = null;

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

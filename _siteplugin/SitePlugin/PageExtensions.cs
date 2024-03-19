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

using Pretzel.Logic.Templating.Context;

namespace SitePlugin
{
    public static class PageExtensions
    {
        public static bool IsPostEstimate( this Page page )
        {
            if( page.Bag.ContainsKey( "is_date_estimate" ) == false )
            {
                return false;
            }
            
            if( bool.TryParse( page.Bag["is_date_estimate"].ToString(), out bool isEstimate ) )
            {
                return isEstimate;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPostEstimate( this PageContext page )
        {
            return page.Page.IsPostEstimate();
        }
    }
}

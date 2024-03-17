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

using System.Text.RegularExpressions;
using Pretzel.Logic.Templating.Context;
using Pretzel.SethExtensions.ImageGallery;

namespace SitePlugin
{
    public class T53GalleryImage
    {
        // ---------------- Fields ----------------

        private static readonly Regex invalidCharacterRegex = new Regex(
            @"[^\w-\.]",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );

        // ---------------- Constructor ----------------

        public T53GalleryImage(
            Page postPage,
            Page galleryThumbnailPage,
            ImageInfoContext context
        )
        {
            this.PretzelImageContext = context;
            this.PostPage = postPage;
            this.GalleryThumbNailPage = galleryThumbnailPage;
            this.Id = invalidCharacterRegex.Replace(
                this.GalleryThumbNailPage.Id,
                "-"
            );
        }

        // ---------------- Properties ----------------

        public string Id { get; }

        public ImageInfoContext PretzelImageContext { get; }

        /// <summary>
        /// The post the photo is a part of.
        /// </summary>
        public Page PostPage { get; }

        /// <summary>
        /// URL to the gallery's thumbnail page.
        /// </summary>
        public Page GalleryThumbNailPage { get; }

        /// <summary>
        /// The time where the photo was modified.
        /// </summary>
        public DateTime OriginalLastModified => 
            this.PretzelImageContext.OriginalPhotoFilePath.LastWriteTimeUtc;

        public int PhotoYear => PostPage.Date.Year;

        public string OriginalPhotoPath => 
            this.PretzelImageContext.OriginalPhotoFilePath.FullName;

        public T53GalleryImage? Previous { get; internal set; } = null;

        public T53GalleryImage? Next { get; internal set; } = null;

        // ---------------- Functions ----------------

        public static bool WasModified( T53GalleryImage cachedImage, ImageInfoContext pretzelContext )
        {
            return cachedImage.OriginalLastModified != pretzelContext.OriginalPhotoFilePath.LastWriteTimeUtc;
        }
    }
}

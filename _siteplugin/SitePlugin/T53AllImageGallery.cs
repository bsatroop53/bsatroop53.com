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

using System.Composition;
using System.Text.RegularExpressions;
using ImageMagick;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;
using Pretzel.SethExtensions.ImageGallery;

namespace SitePlugin
{
    [Export( typeof( IBeforeProcessingTransform ) )]
    public class T53AllImageGallery : IBeforeProcessingTransform
    {
        // ---------------- Fields ----------------

        public static readonly string PageImageGalleryKey = "image_gallery";

        private static readonly Regex whiteSpaceRegex = new Regex(
            @"\s",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture
        );

        private const int maximumWidth = 200;

        private readonly Dictionary<string, T53GalleryImage> images;

        private static readonly Dictionary<int, IReadOnlyList<T53GalleryImage>> imagesByYear;

        // ---------------- Constructor ----------------

        public T53AllImageGallery()
        {
            this.images = new Dictionary<string, T53GalleryImage>();
        }

        static T53AllImageGallery()
        {
            imagesByYear = new Dictionary<int, IReadOnlyList<T53GalleryImage>>();
            ImagesByYear = imagesByYear.AsReadOnly();
        }

        // ---------------- Properties ----------------

        public static int TotalImages { get; private set; }

        public static IReadOnlyDictionary<int, IReadOnlyList<T53GalleryImage>> ImagesByYear { get; }

        // ---------------- Functions ----------------

        public void Transform( SiteContext context )
        {
            Console.WriteLine( "Generating thumbnails for full gallery..." );

            DirectoryInfo baseThumbnailWorkFolder = context.GetThumbnailWorkFolder();
            DirectoryInfo galleryThumbnailWorkFolder = new DirectoryInfo(
                Path.Combine( baseThumbnailWorkFolder.FullName, "full_gallery" )
            );

            if( galleryThumbnailWorkFolder.Exists == false )
            {
                Directory.CreateDirectory( galleryThumbnailWorkFolder.FullName );
            }

            foreach( Page post in context.Posts )
            {
                if( post.Bag.ContainsKey( PageImageGalleryKey ) == false )
                {
                    continue;
                }

                var imagesToProcess = new HashSet<ImageInfoContext>();

                foreach( ImageInfoContext image in post.GetImageGalleryConfig() )
                {
                    string key = image.OriginalPhotoFilePath.FullName;
                    if( this.images.TryGetValue( key, out T53GalleryImage? value ) )
                    {
                        if( T53GalleryImage.WasModified( value, image ) == false )
                        {
                            continue;
                        }

                        // Otherwise, the image was modified, and we need
                        // to regenerate the small thumbnail.
                    }

                    imagesToProcess.Add( image );
                }

                Parallel.ForEach(
                    imagesToProcess,
                    ( ImageInfoContext pretezelContext ) =>
                    {
                        Page thumbnail = CreateGalleryThumbnail( context, post, galleryThumbnailWorkFolder, pretezelContext );

                        var t53GalleryPhoto = new T53GalleryImage(
                            post,
                            thumbnail,
                            pretezelContext
                        );

                        string key = pretezelContext.OriginalPhotoFilePath.FullName;
                        lock( this.images )
                        {
                            this.images[key] = t53GalleryPhoto;
                        }
                    }
                );

                TotalImages = this.images.Count;

                var localByYear = new Dictionary<int, List<T53GalleryImage>>();
                foreach( T53GalleryImage image in this.images.Values )
                {
                    if( localByYear.ContainsKey( image.PhotoYear ) == false )
                    {
                        localByYear[image.PhotoYear] = new List<T53GalleryImage>();
                    }

                    localByYear[image.PhotoYear].Add( image );

                    context.Pages.Add( image.GalleryThumbNailPage );
                }

                foreach( List<T53GalleryImage> imageList in localByYear.Values )
                {
                    imageList.Sort( ( left, right ) => left.PostPage.Date.CompareTo( right.PostPage.Date ) );
                }

                lock( imagesByYear )
                {
                    imagesByYear.Clear();
                    foreach( KeyValuePair<int, List<T53GalleryImage>> imageByYear in localByYear )
                    {
                        imagesByYear[imageByYear.Key] = imageByYear.Value.AsReadOnly();
                    }
                }
            }

            Console.WriteLine( "Generating thumbnails for full gallery... Done!" );
        }

        private static Page CreateGalleryThumbnail(
            SiteContext siteContext,
            Page postContainingPhoto,
            DirectoryInfo workingDirectory,
            ImageInfoContext imageContext
        )
        {
            var linkHelper = new LinkHelper();

            double ratio = maximumWidth / ( (double)imageContext.OriginalWidth );

            if( ratio > 1 )
            {
                ratio = 1;
            }

            string postTitle = whiteSpaceRegex.Replace( postContainingPhoto.Title, "_" );
            string outputFileName = $"{postTitle}_{imageContext.ImageInfo.ThumbnailFileName}";

            int thumbnailHeight = (int)Math.Round( imageContext.OriginalHeight * ratio );
            int thumbnailWidth = Math.Min( maximumWidth, imageContext.OriginalWidth );

            using( var fileStream = new FileStream( imageContext.OriginalPhotoFilePath.FullName, FileMode.Open, FileAccess.Read ) )
            using( var image = new MagickImage( fileStream ) )
            {
                var size = new MagickGeometry( thumbnailWidth, thumbnailHeight );
                image.Resize( size );

                string inFile = Path.Combine( workingDirectory.FullName, imageContext.ImageInfo.ThumbnailFileName );
                image.Write( inFile );

                string outputFile = Path.Combine(
                    siteContext.GetOutputImageDirectory(),
                    "all_photo_gallery",
                    outputFileName
                );

                var thumbnailPage = new Page
                {
                    Id = $"AllPhotosGallery-{postTitle}-{imageContext.ImageInfo.FileName}",
                    File = inFile,
                    Filepath = outputFile,
                    OutputFile = outputFile
                };
                thumbnailPage.Bag["layout"] = "nil";
                thumbnailPage.Url = linkHelper.EvaluateLink( siteContext, thumbnailPage );

                return thumbnailPage;
            }
        }
    }
}

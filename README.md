# NB! This tool is far from finished
 
 I started this project hoping to find an easy way to rename all my photos and videos in a standard format, but I have eventually realised the task is too big, because of all the different ways different camera and phone makers tag their images.
 
 I also have way too many images that don't have any tags at all.
 
 I keep the project as a learning excercise in how to read and write image and video tags in C#.
 
 # How SuperSimplePhotoRenamer works and how to use it

The application renames files with following file extensions: JPG, JPEG, PNG, GIF, MP4, AVI, MOV, MPG, MPEG

The application only renames files *in the directory* the command is run. Not in subfolders.

## File name format

The files are renamed using the date the picture was taken, formatted like this:

* **2016-03-27 19-47-59.jpg** (date, time and lowercase extension)

If there are several files with the same date, the application adds a counter to the end of the file name:

* **2016-03-27 19-47-59.jpg**
* **2016-03-27 19-47-59_1.jpg**
* **2016-03-27 19-47-59_2.jpg**

## Rules for finding date picture was taken

The date used for the file name is found using these rules, using the first matching rule and then ignoring the rest.

1. If the file contains EXIF data for "Date taken", use this date.
2. If the file is anything but a JPEG, it doesn't contain EXIF. Different file types contain different tags for creation date, the app will look for some of the more common ones.
3. If the filename contains a date, use this date.

If no date tag of any kind can be found and the filename doesn't contain a date, the application will not rename the file. Then it is up to the user to manually rename the file to a date, for example based on the date of the photos taken at the same time.

## Parsing dates in filename

To find the date in the filename, the application takes the first 14 digits in the filename (ignoring any non-digits in between) and tries to parse that as a date.

This means it will find the date in filenames such as:
* IMG_20160315_145027.JPG
* 2016-03-15 14-50-27.JPG
* IMAGE 20160315145027 MEDIA 123.JPEG

But it will not be able to parse dates without time (such as 2016-03-15) or dates where month or day comes before year (such as 15-03-2016 or 03-15-2016).

## Ignore already renamed files

If a file is already renamed, ie. already has the expected format, the application will ignore it and don't log anything. If the only change to a file is change to the file extension, the rename will still be logged.

## Log renamed files

All renames are logged in a log file in the directory the command is run, called **supersimplephotorenamer.log**. The file is created if it doesn't exist, otherwise the application appends to the end of it.

The log has the following format (including the headers):

    Logged                 New filename                 Date source      Original filename
    2016-06-14 13-37-44    2016-03-15 14-50-27.jpg      Filename         IMAGE 20160315145027 MEDIA 123.JPEG
    2016-06-14 13-37-45    2016-03-15 14-50-27_1.jpg    Filename         20160315145027.jpg
    2016-06-14 13-37-46    2016-03-15 14-50-27_2.mpg    Modified date    MOVIE.MPEG
    2016-06-14 13-37-46    2016-03-15 14-50-27_3.mpg    EXIF             Picture.jpg

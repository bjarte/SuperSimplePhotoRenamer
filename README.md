#How this program works

The program will look up all files of the following types:
- JPG/JPEG
- PNG
- GIF
- MP4
- AVI
- MOV

## File name format

The files will be renamed with the date the picture was taken, formatted like this:

* **2016-03-27 19-47-59.jpg**
* **year-month-date hours-minutes-seconds.lowercaseextension**

If there are several files with the same date, add a counter to the end like this:

* **2016-03-27 19-47-59.jpg**
* **2016-03-27 19-47-59_1.jpg**
* **2016-03-27 19-47-59_2.jpg**

## Rules for finding date picture was taken

The date used for the file name is found using the following rules, using the first matching rule and then ignoring the rest.

1. If the filename contains a date, use this date. Examples:
  * IMG_20160315_145027.JPG
  * 2016-03-15 14-50-27.JPG
  * IMAGE 20160315145027 MEDIA.JPEG
2. If the file contains EXIF data for "Date taken", use this date.
3. If no other information is found, use the date from file property "Modified". This is not very reliable, but more reliable than "Created" date.
  

REM Do not overwrite existing directory
IF exist Subject-%1 goto finished

mkdir InfiniteArtGallery_PROTOTYPE_v0.05.1\Subject-%1
START InfiniteArtGallery_PROTOTYPE_v0.05.1\InfiniteArtGallery.exe -testerID %1
START IAG-Record.bat Subject-%1

ECHO "Press a key after your match to take the survey"
PAUSE
REM Survey 2
START "" https://goo.gl/forms/kDXfKoy6kWhxtKWz1

:finished
ECHO "DONE"
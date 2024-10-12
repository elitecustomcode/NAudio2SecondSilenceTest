# NAudio 2 Second Silence Test
This is a console application that checks if a mp3 file has at least 2 seconds conecutive zero audio (silence). If so the file will be flagged and afterwards reportet into a text file on the desktop of the current user.
On startup, you have to provide a path to the audio files folder and set the max threads the system should run with. Depends on the CPU beteween 5 and 10 threads is fine.

# Makefile

FILES = file_server file_client
SOURCES = $(addsuffix .cs,${FILES})
EXE = $(addsuffix .exe,${FILES})

.PHONY: all
all: ${EXE}

%.exe: %.cs
	csc $<
	chmod 777 $@

.PHONY: clean

clean:
	rm -f ${EXE}
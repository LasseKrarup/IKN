# Makefile

FILES = UDPserver UDPclient
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
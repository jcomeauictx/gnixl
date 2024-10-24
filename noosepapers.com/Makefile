SHELL := /bin/bash
SERVER := $(notdir $(PWD))
DOCROOT := /var/www
DRYRUN ?= --dry-run
PATH := /usr/local/casperscript/bin:$(PATH)
TIMESTAMP := $(shell date +'%Y%m%d%H%M%S')
PDFS := $(wildcard *.pdf)
TXTS := $(PDFS:.pdf=.txt)
TEXTFILE ?= $(word 1, $(TXTS))
all: columns.log
gallows.pgm: gallows.png
	pngtopnm $< | pnmnoraw > $@
%.log: %.pgm ../../casperscript .FORCE
	[ -f $@ ] && mv -f $@ $*.$(TIMESTAMP).log
	cs -- ../../casperscript/lib/viewpbm.ps $+ | tee $@
%.gv: %.cs noosefonts.ps
	/usr/bin/gs -sFONTMAP=$(PWD)/noosefonts.ps -I $(PWD) <(tail -n +2 $<)
%.log: %.cs noosefonts.ps $(TXTS) .FORCE
	([ -f $@ ] && mv -f $@ $*.$(TIMESTAMP).log) || true
	./$< $(TEXTFILE) | tee $@
../../casperscript:
	@echo Must git clone casperscript to the grandparent directory. >&2
	false
upload:
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 . $(SERVER):$(DOCROOT)/$(SERVER)/
%.txt: %.pdf Makefile
	pdftotext -nopgbrk -eol unix -enc Latin1 $<
edit:
	vi columns.cs
.PRECIOUS: %.ps
.FORCE:

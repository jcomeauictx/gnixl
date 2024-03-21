SHELL := /bin/bash
SERVER := $(notdir $(PWD))
DOCROOT := /var/www
DRYRUN ?= --dry-run
PATH := /usr/local/casperscript/bin:$(PATH)
all: banner.view
gallows.pgm: gallows.png
	pngtopnm $< | pnmnoraw > $@
%.view: %.pgm ../../casperscript
	cs -- ../../casperscript/lib/viewpbm.ps $<
%.gv: %.cs noosefonts.ps
	/usr/bin/gs -sFONTMAP=$(PWD)/noosefonts.ps -I $(PWD) <(tail -n +2 $<)
%.view: %.cs noosefonts.ps
	./$<
../casperscript:
	@echo Must git clone casperscript to the parent directory. >&2
	false
upload:
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 . $(SERVER):$(DOCROOT)/$(SERVER)/
%.txt: %.pdf
	pdftotext $<
.PRECIOUS: %.ps

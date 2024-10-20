SHELL := /bin/bash
SERVER := $(notdir $(PWD))
DOCROOT := /var/www
DRYRUN ?= --dry-run
PATH := /usr/local/casperscript/bin:$(PATH)
all: banner.log
gallows.pgm: gallows.png
	pngtopnm $< | pnmnoraw > $@
%.log: %.pgm ../../casperscript
	cs -- ../../casperscript/lib/viewpbm.ps $< | tee $@
%.gv: %.cs noosefonts.ps
	/usr/bin/gs -sFONTMAP=$(PWD)/noosefonts.ps -I $(PWD) <(tail -n +2 $<)
%.log: %.cs noosefonts.ps
	./$< | tee $@
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

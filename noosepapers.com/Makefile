SERVER := $(notdir $(PWD))
DOCROOT := /var/www
DRYRUN ?= --dry-run
gallows.pgm: gallows.png
	pngtopnm $< | pnmnoraw > $@
%.ps: %.ps.template
	envsubst < $< > $@
%.view: %.pgm ../../casperscript
	cs -- ../../casperscript/lib/viewpbm.ps $<
%.view: %.cs noosefonts.ps
	cs $< -sFONTMAP=$(PWD)/noosefonts.ps -sFONTPATH=$(PWD) $<
../casperscript:
	@echo Must git clone casperscript to the parent directory. >&2
	false
upload:
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 . $(SERVER):$(DOCROOT)/$(SERVER)/
.PRECIOUS: %.ps

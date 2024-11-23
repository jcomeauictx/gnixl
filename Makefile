SHELL := /bin/bash
WEBSITES := $(wildcard *.com *.net *.org)
WEBSITES += flyxwyrk.com w-a-s-t-e.org
WEBSITES += kybyz.com kybytz.com umanure.com umanure.net
UPLOADS: $(addsuffix .upload, $(WEBSITES))
SERVER := $(notdir $(PWD))
DRYRUN ?= --dry-run
DOCROOT := /var/www
uploads: $(UPLOADS)
%.upload: %/Makefile
	cd $(<D) && $(MAKE) upload
%.upload: ../*/%/Makefile
	@echo Found Makefile at $<
	cd $(<D) && $(MAKE) upload
# the following rule assumes symlinks to this Makefile from website dirs
upload:
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 --exclude='*.log' \
	 --exclude='*.err' \
	 --exclude='.gitignore' \
	 . $(SERVER):$(DOCROOT)/$(SERVER)/

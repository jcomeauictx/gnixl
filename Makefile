SHELL := /bin/bash
WEBSITES := $(wildcard *.com *.net *.org)
UPLOADS: $(addsuffix .upload, $(WEBSITES))
SERVER := $(notdir $(PWD))
DRYRUN ?= --dry-run
uploads: $(UPLOADS)
%.upload: %
	cd $< && $(MAKE) upload
# the following rule assumes symlinks to this Makefile from website dirs
upload:
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 . root@$(SERVER):$(DOCROOT)/$(SERVER)

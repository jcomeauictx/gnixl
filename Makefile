SHELL := /bin/bash
WEBSITES := $(wildcard *.com *.net *.org)
UPLOADS: $(addsuffix .upload, $(WEBSITES))
DRYRUN ?= --dry-run
upload: $(UPLOADS)
%.upload: %
	cd $< && $(MAKE) upload

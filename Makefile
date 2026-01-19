SHELL := /bin/bash
WEBSITES := $(wildcard *.com *.net *.org)
WEBSITES += flyxwyrk.com w-a-s-t-e.org
WEBSITES += kybyz.com kybytz.com umanure.com umanure.net
UPLOADS: $(addsuffix .upload, $(WEBSITES))
SERVER := $(notdir $(PWD))
DRYRUN ?= --dry-run
DOCROOT := /var/www
SYMLINKS := $(shell find /var/www/ -maxdepth 1 -type l)
SUBDOMAINS := $(wildcard /var/www/*.*.*)
DOMAINS := $(notdir $(filter-out $(SYMLINKS) $(SUBDOMAINS), \
	   $(wildcard /var/www/*.*)))
WWW := $(addprefix www., $(DOMAINS))
RENEW := $(shell echo $(DOMAINS) $(WWW) | tr ' ' ',')
DRYRUN ?= --dry-run
ifneq ($(SHOWENV),)
	export
endif
uploads: $(UPLOADS)
%.upload: %/Makefile
	cd $(<D) && $(MAKE) upload
%.upload: ../*/%/Makefile
	@echo Found Makefile at $<
	cd $(<D) && $(MAKE) upload
# the following rule assumes symlinks to this Makefile from website dirs
upload:
	if [ "$(SERVER)" = gnixl ]; then \
	 false; \
	fi  # quit if attempting to use from root directory of repo
	rsync -avuz $(DRYRUN) $(DELETE) \
	 --exclude='Makefile' \
	 --exclude='README.md' \
	 --exclude='*.log' \
	 --exclude='*.err' \
	 --exclude='.gitignore' \
	 . $(SERVER):$(DOCROOT)/$(SERVER)/
renew:
	@echo renewing $(DOMAINS) $(WWW) >&2
	sudo certbot certonly $(DRYRUN) \
	 --key-type rsa \
	 --cert-name certbot_cert \
	 --apache \
	 --expand \
	 --domains $(RENEW)
env:
ifeq ($(SHOWENV),)
	$(MAKE) SHOWENV=1 $@
else
	@echo showing environment variables >&2
	$@
endif
.PHONY: env

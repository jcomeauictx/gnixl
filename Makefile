SERVER := gnixl.com
DOCROOT := /var/www/
DRYRUN ?= --dry-run

upload:
	rsync -avuz \
	 $(DRYRUN) \
	 --exclude=.git \
	 --exclude=Makefile \
	 --exclude=README.md \
	 . $(SERVER):$(DOCROOT)/

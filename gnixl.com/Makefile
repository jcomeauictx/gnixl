SERVER := gnixl.com
DOCROOT := /var/www/$(SERVER)
DRYRUN ?= --dry-run
NOTLINKS := l/README.md l/index.html
LINKS := $(filter-out $(NOTLINKS), $(wildcard l/*))

upload:
	rsync -avuz \
	 $(DRYRUN) \
	 --exclude=.git \
	 --exclude=Makefile \
	 --exclude=README.md \
	 . $(SERVER):$(DOCROOT)/
clean:
distclean: clean
	# l/.htaccess shouldn't even be there, it doesn't work
	rm -rf dummy $(LINKS) .htaccess l/.htaccess

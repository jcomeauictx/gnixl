gallows.pgm: gallows.png
	pngtopnm $< | pnmnoraw > $@
%.view: %.pgm ../../casperscript
	cs -- ../../casperscript/lib/viewpbm.ps $<
../casperscript:
	@echo Must git clone casperscript to the parent directory. >&2
	false

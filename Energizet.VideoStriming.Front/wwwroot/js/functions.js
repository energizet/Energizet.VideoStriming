(() => {
	function addOption(child, key, option) {
		switch (key) {
			case 'class':
				if (!Array.isArray(option)) {
					option = option.split(' ');
				}

				option.forEach(item => {
					if (item !== '') {
						child.classList.add(item);
					}
				});
				break;
			case 'on':
				Object.keys(option).forEach(eventName => {
					child['on' + eventName] = option[eventName];
				});
				break;
			case 'children':
				child.addCustomElements(option);
				break;
			default:
				child[key] = option;
				break;
		}
	}

	Element.prototype.addCustomElements = function (elements) {
		elements.forEach(element => {
			if (element instanceof Element) {
				this.appendChild(element);
			} else {
				let view = this.addCustomElement(element[0], element[1]);
				this.appendChild(view);
			}
		});
	};

	Element.prototype.addCustomElement = function (tag, options = undefined) {
		let child = document.createCustomElement(tag, options);
		this.appendChild(child);
		return child;
	};

	Document.prototype.createCustomElement = function (tag, options = undefined) {
		if (tag instanceof Element) {
			return tag;
		}

		let child = document.createElement(tag);
		if (typeof options == 'object') {
			Object.keys(options).forEach(key => addOption(child, key, options[key]));
		}
		return child;
	};
})();
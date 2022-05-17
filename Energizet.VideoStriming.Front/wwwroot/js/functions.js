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

	Element.prototype.addCustomElement = function (tag, options = null) {
		let child = document.createElement(tag);
		if (typeof options == 'object') {
			Object.keys(options).forEach(key => addOption(child, key, options[key]));
		}
		this.appendChild(child);
		return child;
	};

	Element.prototype.addCustomElements = function (elements) {
		elements.forEach(element => {
			let view = this.addCustomElement(element[0], element[1]);
			this.appendChild(view);
		});
	};
})();
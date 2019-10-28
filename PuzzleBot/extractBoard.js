let info = $('.puzzleInfo p')[0].innerText;
let regex = /(\d+)x(\d+)/g;
let match = regex.exec(info);
let height = parseInt(match[2]), width = parseInt(match[1]);

let counter = 0;
let string = height + " " + width + '\n';
$.each($('.loop-task-cell'), (i, p) => 
{
	if(counter == width){
		string += '\n';
		counter = 0;
	}
	let frag = $(p).html();
	if(frag) {
		string += (frag + " ");
	} else {
		string += ("4 ");
	}
	
	counter++;
})

console.log(string);
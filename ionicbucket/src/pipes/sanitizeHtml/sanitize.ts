import { Pipe } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'sanitize',
  pure: true
})
export class Sanitize {

  constructor(private domSanitizer: DomSanitizer) { }


  handleExternalScriptsInHtmlString(string) {
    let that = this;
    var parser = new DOMParser();
    var scripts = parser.parseFromString(string, 'text/html').getElementsByTagName('script');
    console.log(scripts);
    var results = [];

    for (var i = 0; i < scripts.length; i++) { 
      that.addScript(scripts[i].innerText);
      scripts[i].remove();
    }
  }

  addScript(src) {
    var script = document.createElement('script');
    script.innerHTML = src;
    document.getElementById("threed").appendChild(script);
  }


  transform(htmlContent: any) {
    if(htmlContent!=null )
     {
      let sanitizeHtmlContent = this.domSanitizer.bypassSecurityTrustHtml(htmlContent);
      return sanitizeHtmlContent;
     }
     return null;
  }
}
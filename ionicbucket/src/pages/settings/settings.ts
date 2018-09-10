/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Settings Component
 * File path - '../../../src/pages/settings/settings'
 */

import { Component } from '@angular/core';
import { IonicPage, Platform } from 'ionic-angular';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';

@IonicPage()
@Component({
  selector: 'page-settings',
  templateUrl: 'settings.html',
})
export class SettingsPage {

  languages: any;

  constructor(
    private http: HttpClient,
    public platform: Platform,
    public translateService: TranslateService) {
  }

  /** Do any initialization */
  ngOnInit() {
    this.getAllLanguages();
  }

  /**
   * -------------------------------------------------------------------
   * Get All Languages
   * -------------------------------------------------------------------
   * @method getAllLanguages
   */
  getAllLanguages() {
    this.http.get('assets/i18n/en.json').subscribe((languages: any) => {
      this.languages = languages.LANGUAGES;
    }, error => {
      console.error('Error: ' + error);
    });
  }

  /**
   * -------------------------------------------------------------------
   * Choose Language
   * -------------------------------------------------------------------
   * @method changeLanguage
   * @param language      Selected Language
   * 
   * When language code 'ar' then the platform direction will be 'rtl' otherwise platform direction 'ltr' 
   */
  changeLanguage(language) {
    if (language === 'ar') {
      this.platform.setDir('rtl', true);
      this.translateService.setDefaultLang(language);
    } else {
      this.platform.setDir('ltr', true);
      this.translateService.setDefaultLang(language);
    }
  }
}

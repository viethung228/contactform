/**
 * initialize selection setting panel
 */

function getKeyByValue(object, value) {
    return Object.keys(object).find(key => object[key] === value);
}

(function () {
    'use strict';
    const mainColors = {
        color1: "#000", //black
        color2: "#fff", //white
        color3: "#f22613", //red
        color4: "#f9690e", //orange
        color5: "#F9EA3A", //yellow
        color6: "#1e8bc3", //blue
        color7: "#049372", //green
        color8: "#8e44ad" //purple
    };

  const BorderStyleList = [{
    value: {
      strokeDashArray: [],
      strokeLineCap: 'butt'
    },
    label: "Stroke"
  }, {
    value: {
      strokeDashArray: [1, 10],
      strokeLineCap: 'butt'
    },
    label: 'Dash-1'
  }, {
    value: {
      strokeDashArray: [1, 10],
      strokeLineCap: 'round'
    },
    label: 'Dash-2'
  }, {
    value: {
      strokeDashArray: [15, 15],
      strokeLineCap: 'square'
    },
    label: 'Dash-3'
  }, {
    value: {
      strokeDashArray: [15, 15],
      strokeLineCap: 'round'
    },
    label: 'Dash-4'
  }, {
    value: {
      strokeDashArray: [25, 25],
      strokeLineCap: 'square'
    },
    label: 'Dash-5',
  }, {
    value: {
      strokeDashArray: [25, 25],
      strokeLineCap: 'round'
    },
    label: 'Dash-6',
  }, {
    value: {
      strokeDashArray: [1, 8, 16, 8, 1, 20],
      strokeLineCap: 'square'
    },
    label: 'Dash-7',
  }, {
    value: {
      strokeDashArray: [1, 8, 16, 8, 1, 20],
      strokeLineCap: 'round'
    },
    label: 'Dash-8',
  }]
  const AlignmentButtonList = [{
    pos: 'left',
    desc: '左に揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g transform="translate(1.4305e-6 -17.438)" stroke-width="1.2346"><rect x="14.815" y="48.16" width="85.185" height="24.691"></rect><rect x="14.815" y="87.025" width="45.679" height="24.691"></rect><rect y="34.877" width="8.642" height="90.123"></rect></g></svg>`
  }, {
    pos: 'center-h',
    desc: '水平方向に中央揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g stroke-width="1.2346"><rect x="7.4075" y="30.722" width="85.185" height="24.691"></rect><rect x="27.16" y="69.587" width="45.679" height="24.691"></rect><rect x="45.679" y="17.439" width="8.642" height="90.123"></rect></g></svg>`,
  }, {
    pos: 'right',
    desc: '右に揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g transform="translate(1.4305e-6 -17.438)" stroke-width="1.2346"><rect transform="scale(-1,1)" x="-85.185" y="48.16" width="85.185" height="24.691"></rect><rect transform="scale(-1,1)" x="-85.185" y="87.025" width="45.679" height="24.691"></rect><rect transform="scale(-1,1)" x="-100" y="34.877" width="8.642" height="90.123"></rect></g></svg>`,
  }, {
    pos: 'top',
    desc: '上端に揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g transform="translate(1.4305e-6 -17.438)"><g transform="matrix(0 -1 -1 0 129.94 129.94)" stroke-width="1.2346"><rect transform="scale(-1,1)" x="-85.185" y="48.16" width="85.185" height="24.691"></rect><rect transform="scale(-1,1)" x="-85.185" y="87.025" width="45.679" height="24.691"></rect><rect transform="scale(-1,1)" x="-100" y="34.877" width="8.642" height="90.123"></rect></g></g></svg>`,
  }, {
    pos: 'center-v',
    desc: '垂直方向に中央揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g stroke-width="1.2346"><rect transform="rotate(90)" x="19.908" y="-81.779" width="85.185" height="24.691"></rect><rect transform="rotate(90)" x="39.66" y="-42.913" width="45.679" height="24.691"></rect><rect transform="rotate(90)" x="58.179" y="-95.062" width="8.642" height="90.123"></rect></g></svg>`
  }, {
    pos: 'bottom',
    desc: '下端に揃え',
    icon: `<svg enable-background="new 0 0 100 100" viewBox="0 0 100 125" xml:space="preserve"><g transform="translate(1.4305e-6 -17.438)"><g transform="rotate(90 50 79.938)" stroke-width="1.2346"><rect transform="scale(-1,1)" x="-85.185" y="48.16" width="85.185" height="24.691"></rect><rect transform="scale(-1,1)" x="-85.185" y="87.025" width="45.679" height="24.691"></rect><rect transform="scale(-1,1)" x="-100" y="34.877" width="8.642" height="90.123"></rect></g></g></svg>`
  }]
  var selectionSettings = function () {
      const _self = this;
      $(`${this.containerSelector} .main-panel`).append(`<div class="toolpanel" id="select-panel"><div class="content"><p class="title">Selection Settings</p></div>    <div class="panel-dismiss-wrapper"><button type="button" class="panel-dismiss btn btn-sm btn-secondary">Close</button></div></div>`);

    // font section
    (() => {
        var panelPropFont = {
            FontStyle: "フォントのスタイル",
            FontFamily: "フォントファミリー",
            FontSize: "フォントのサイズ",
            LineHeight: "テキストの行間の高さ",
            LetterSpacing: "テキストの字間のスペース",
            TextAlignment: "テキストの配置",
            TextColor: "テキストの色"
        };

      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="text-section">
          <h4>${panelPropFont.FontStyle}</h4>
          <div class="style">
            <button type="button" id="bold"><svg id="Capa_1" x="0px" y="0px" viewBox="-70 -70 450 450" xml:space="preserve"><path d="M218.133,144.853c20.587-14.4,35.2-37.653,35.2-59.52C253.333,37.227,216.107,0,168,0H34.667v298.667h150.187 c44.693,0,79.147-36.267,79.147-80.853C264,185.387,245.547,157.76,218.133,144.853z M98.667,53.333h64c17.707,0,32,14.293,32,32 s-14.293,32-32,32h-64V53.333z M173.333,245.333H98.667v-64h74.667c17.707,0,32,14.293,32,32S191.04,245.333,173.333,245.333z"></path></svg></button>
            <button type="button" id="italic"><svg id="Capa_1" x="0px" y="0px" viewBox="-70 -70 450 450" xml:space="preserve"><polygon points="106.667,0 106.667,64 153.92,64 80.747,234.667 21.333,234.667 21.333,298.667 192,298.667 192,234.667 144.747,234.667 217.92,64 277.333,64 277.333,0  "></polygon></svg></button>
            <button type="button" id="underline"><svg id="Capa_1" x="0px" y="0px" viewBox="-70 -70 450 450" xml:space="preserve"><path d="M192,298.667c70.72,0,128-57.28,128-128V0h-53.333v170.667c0,41.28-33.387,74.667-74.667,74.667 s-74.667-33.387-74.667-74.667V0H64v170.667C64,241.387,121.28,298.667,192,298.667z"></path><rect x="42.667" y="341.333" width="298.667" height="42.667"></rect></svg></button>
            <button type="button" id="linethrough"><svg id="Capa_1" x="0px" y="0px" viewBox="-70 -70 450 450" xml:space="preserve"><polygon points="149.333,160 234.667,160 234.667,96 341.333,96 341.333,32 42.667,32 42.667,96 149.333,96"></polygon><rect x="149.333" y="288" width="85.333" height="64"></rect><rect x="0" y="202.667" width="384" height="42.667"></rect></svg></button>
            <button type="button" id="subscript"><svg id="Capa_1" x="0px" y="0px" viewBox="0 0 512 512" xml:space="preserve"><path d="M248.257,256l103.986-103.758c2.777-2.771,4.337-6.532,4.337-10.455c0-3.923-1.561-7.684-4.337-10.455l-49.057-48.948 c-5.765-5.753-15.098-5.753-20.863,0L178.29,186.188L74.258,82.384c-5.764-5.751-15.098-5.752-20.863,0L4.337,131.333 C1.561,134.103,0,137.865,0,141.788c0,3.923,1.561,7.684,4.337,10.455L108.324,256L4.337,359.758 C1.561,362.528,0,366.29,0,370.212c0,3.923,1.561,7.684,4.337,10.455l49.057,48.948c5.765,5.753,15.098,5.753,20.863,0 l104.033-103.804l104.032,103.804c2.883,2.876,6.657,4.315,10.432,4.315s7.549-1.438,10.432-4.315l49.056-48.948 c2.777-2.771,4.337-6.532,4.337-10.455c0-3.923-1.561-7.684-4.337-10.455L248.257,256z"></path><path d="M497.231,384.331h-44.973l35.508-31.887c14.878-13.36,20.056-34.18,13.192-53.04 c-6.874-18.89-23.565-31.044-43.561-31.717c-0.639-0.021-1.283-0.032-1.928-0.032c-31.171,0-56.531,25.318-56.531,56.439 c0,8.157,6.613,14.769,14.769,14.769c8.156,0,14.769-6.613,14.769-14.769c0-14.833,12.109-26.901,26.992-26.901 c0.316,0,0.631,0.005,0.937,0.016c11.573,0.39,15.78,9.511,16.795,12.297c2.163,5.946,1.942,14.574-5.171,20.962l-64.19,57.643 c-4.552,4.088-6.112,10.56-3.923,16.273c2.189,5.714,7.673,9.486,13.792,9.486h83.523c8.157,0,14.769-6.613,14.769-14.769 S505.387,384.331,497.231,384.331z"></path></svg></button>
            <button type="button" id="superscript"><svg id="Capa_1" x="0px" y="0px" viewBox="0 0 512 512" xml:space="preserve"><path d="M248.257,259.854l103.986-103.758c2.777-2.771,4.337-6.532,4.337-10.455c0-3.923-1.561-7.684-4.337-10.455l-49.057-48.948 c-5.765-5.753-15.098-5.753-20.863,0L178.29,190.042L74.258,86.238c-5.764-5.751-15.099-5.752-20.863,0L4.337,135.187 C1.561,137.958,0,141.719,0,145.642s1.561,7.684,4.337,10.455l103.986,103.758L4.337,363.612C1.561,366.383,0,370.145,0,374.067 c0,3.922,1.561,7.684,4.337,10.455l49.057,48.948c5.765,5.753,15.098,5.753,20.863,0l104.033-103.804l104.032,103.804 c2.883,2.876,6.657,4.315,10.432,4.315s7.549-1.438,10.432-4.315l49.056-48.948c2.777-2.771,4.337-6.532,4.337-10.455 s-1.561-7.684-4.337-10.455L248.257,259.854z"></path><path d="M497.231,190.893h-44.973l35.508-31.887c14.878-13.36,20.056-34.18,13.192-53.04 c-6.874-18.89-23.565-31.044-43.561-31.717c-0.639-0.021-1.283-0.032-1.928-0.032c-31.171,0-56.531,25.318-56.531,56.439 c0,8.157,6.613,14.769,14.769,14.769c8.156,0,14.769-6.613,14.769-14.769c0-14.833,12.109-26.901,26.992-26.901 c0.316,0,0.631,0.005,0.937,0.016c11.573,0.39,15.78,9.511,16.795,12.297c2.163,5.946,1.942,14.574-5.171,20.962l-64.19,57.643 c-4.552,4.088-6.112,10.56-3.923,16.273c2.189,5.714,7.673,9.486,13.792,9.486h83.523c8.157,0,14.769-6.613,14.769-14.769 S505.387,190.893,497.231,190.893z"></path></svg></button>
          </div>
          <div class="family">
            <div class="input-container">
            <label>${panelPropFont.FontFamily}</label>
            <select id="font-family">
              <option value="Open Sans">Open Sans</option>
              <option value="KsoYose">KsoYose</option>
              <option value="KsoYuai">KsoYuai</option>
              <option value="KsoYujin">KsoYujin</option>
              <option value="KsoYurei">KsoYurei</option>
            </select>
            </div>
          </div>
          <div class="sizes">
            <div class="input-container"><label>${panelPropFont.FontSize}</label>
              <div class="custom-number-input">
              <button type="button" class="decrease">-</button>
              <input type="number" min="1" value="20" id="fontSize">
              <button type="button" class="increase">+</button>
              </div>
            </div>
            <div class="input-container"><label>${panelPropFont.LineHeight}</label>
              <div class="custom-number-input">
              <button type="button" class="decrease">-</button>
              <input type="number" min="0" max="3" value="1" step="0.1" id="lineHeight">
              <button type="button" class="increase">+</button>
              </div>
            </div>
            <div class="input-container"><label>${panelPropFont.LetterSpacing}</label>
              <div class="custom-number-input">
              <button type="button" class="decrease">-</button>
              <input type="number" min="0" max="2000" step="100" value="0" id="charSpacing">
              <button type="button" class="increase">+</button>
              </div>
            </div>
            </p>
          </div>
          <div class="align">
            <div class="input-container">
            <label>${panelPropFont.TextAlignment}</label>
            <select id="text-align">
              <option value="left">Left</option>
              <option value="center">Center</option>
              <option value="right">Right</option>
              <option value="justify">Justify</option>
            </select>
            </div>
          </div>
          <div class="color">
            <div class="input-container">
            <label>${panelPropFont.TextColor}</label>
            <input id="color-picker" value="black">
            </div>
          </div>
          <hr>
        </div>
      `);
      $(`${this.containerSelector} .toolpanel#select-panel .style button`).click(function () {
        let type = $(this).attr('id');
        switch (type) {
          case 'bold':
            setActiveFontStyle(_self.activeSelection, 'fontWeight', getActiveFontStyle(_self.activeSelection, 'fontWeight') === 'bold' ? '' : 'bold')
            break;
          case 'italic':
            setActiveFontStyle(_self.activeSelection, 'fontStyle', getActiveFontStyle(_self.activeSelection, 'fontStyle') === 'italic' ? '' : 'italic')
            break;
          case 'underline':
            setActiveFontStyle(_self.activeSelection, 'underline', !getActiveFontStyle(_self.activeSelection, 'underline'))
            break;
          case 'linethrough':
            setActiveFontStyle(_self.activeSelection, 'linethrough', !getActiveFontStyle(_self.activeSelection, 'linethrough'))
            break;
          case 'subscript':
            if (getActiveFontStyle(_self.activeSelection, 'deltaY') > 0) {
              setActiveFontStyle(_self.activeSelection, 'fontSize', undefined)
              setActiveFontStyle(_self.activeSelection, 'deltaY', undefined)
            } else {
              _self.activeSelection.setSubscript()
              _self.canvas.renderAll()
            }
            break;
          case 'superscript':
            if (getActiveFontStyle(_self.activeSelection, 'deltaY') < 0) {
              setActiveFontStyle(_self.activeSelection, 'fontSize', undefined)
              setActiveFontStyle(_self.activeSelection, 'deltaY', undefined)
            } else {
              _self.activeSelection.setSuperscript()
              _self.canvas.renderAll()
            }
            break;
          default:
            break;
        }
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
      })

      //$(`${this.containerSelector} .toolpanel#select-panel .family #font-family`).change(function () {
      //  let family = $(this).val();
      //  setActiveFontStyle(_self.activeSelection, 'fontFamily', family)
      //  _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
      //})

    function loadAndUse(font) {
        var myfont = new FontFaceObserver(font);
        myfont.load()
            .then(function () {
                // when font is loaded, use it.
                _self.canvas.getActiveObject().set("fontFamily", font);
                _self.canvas.requestRenderAll();
            }).catch(function (e) {
                console.log(e)
                console.log('font loading failed ' + font);
            });
        }

    $(`${this.containerSelector} .toolpanel#select-panel .family #font-family`).change(function () {
        let family = $(this).val();
        if (family !== 'Open Sans') {
            loadAndUse(family);
        } else {
            _self.canvas.getActiveObject().set("fontFamily", family);
            _self.canvas.requestRenderAll();
        }
        //setActiveFontStyle(_self.activeSelection, 'fontFamily', family)
        //_self.canvas.renderAll(), _self.canvas.trigger('object:modified');
    })

      $(`${this.containerSelector} .toolpanel#select-panel .sizes input`).change(function () {
        let value = parseFloat($(this).val());
        let type = $(this).attr('id');
        setActiveFontStyle(_self.activeSelection, type, value);
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
      })

      $(`${this.containerSelector} .toolpanel#select-panel .align #text-align`).change(function () {
        let mode = $(this).val();
        setActiveFontStyle(_self.activeSelection, 'textAlign', mode);
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
      })

      $(`${this.containerSelector} .toolpanel#select-panel .color #color-picker`).spectrum({
        type: "color",
        showInput: "true",
        allowEmpty: "false"
      });

      $(`${this.containerSelector} .toolpanel#select-panel .color #color-picker`).change(function () {
        let color = $(this).val();
        setActiveFontStyle(_self.activeSelection, 'fill', color)
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
      })
    })();
    // end font section

    // border section
    (() => {
        var panelPropBorder = {
            Border: "境界",
            Width: "幅",
            Style: "スタイル",
            CornerType: "外側の角",
            Color: "幅の色",
            CustomColorPicker: "custom color"
        };

      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="border-section">
          <h4>${panelPropBorder.Border}</h4>
          <div class="input-container"><label>${panelPropBorder.Width}</label>
            <div class="custom-number-input">
            <button type="button" class="decrease">-</button>
            <input type="number" min="1" value="1" id="input-border-width">
            <button type="button" class="increase">+</button>
            </div>
          </div>
          <div class="input-container"><label>${panelPropBorder.Style}</label><select id="input-border-style">${BorderStyleList.map(item => `<option value='${JSON.stringify(item.value)}'>${item.label}</option>`)}</select></div>
          <div class="input-container"><label>${panelPropBorder.CornerType}</label><select id="input-corner-type"><option value="miter" selected>Square</option><option value="round">Round</option></select></div>
          <div class="input-container"><label>${panelPropBorder.Color}</label>
            <div class="color-display-wrapper">
                <div class="color-display" data-color="${mainColors.color1}" style="background:${mainColors.color1}">currentcolor</div>
                <button type="button" class="color-display-toggle" id="color-display-toggle"><i class="fa fa-angle-down" aria-hidden="true"></i></i></button>
            </div>
            <div class="color-panel">
                <div class="color-panel-content">
                    <input id="color-picker" class="hidden" value="${mainColors.color1}" data-default="black">
                    <div class="colors">
	                    <button type="button" class="color regular-color main is-selected" data-color="${mainColors.color1}" style="background:${mainColors.color1}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color2}" style="background:${mainColors.color2}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color3}" style="background:${mainColors.color3}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color4}" style="background:${mainColors.color4}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color5}" style="background:${mainColors.color5}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color6}" style="background:${mainColors.color6}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color7}" style="background:${mainColors.color7}"></button>
	                    <button type="button" class="color regular-color main" data-color="${mainColors.color8}" style="background:${mainColors.color8}"></button>             
                    </div>
                    <div class="color-control-panel">
                        <button type="button" class="color-ctrl edit" id="custom-color-mixer"><i class="fa fa-tint" aria-hidden="true"></i></button>
                        <button type="button" class="color-ctrl save" id="custom-color-save"><i class="fa fa-plus" aria-hidden="true"></i></button>
                        <button type="button" class="color-ctrl reset" data-color="${mainColors.color1}"><i class="fa fa-undo" aria-hidden="true"></i></button>
                        <input id="custom-color-picker" type="color" class="custom-color-picker"/>
                    </div>
                </div>                
            </div>          
          </div>
          <hr>
        </div>
      `);

      //$(`${this.containerSelector} .toolpanel#select-panel .border-section #color-picker`).spectrum({
      //  showButtons: false,
      //  type: "color",
      //  showInput: "true",
      //  allowEmpty: "false",
      //  move: function (color) {
      //    let hex = 'transparent';
      //    color && (hex = color.toRgbString()); // #ff0000
      //    _self.canvas.getActiveObjects().forEach(obj => obj.set('stroke', hex))
      //    _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      //  }
      //});

        $(`${this.containerSelector} .toolpanel#select-panel .border-section #custom-color-picker`).on("input", function () {
            let colorBtn = $(this);

            $(`.toolpanel#select-panel .border-section .colors .color`).removeClass("is-selected");

            $(`.toolpanel#select-panel .border-section #color-picker`).val(colorBtn.val());

            let colorHex = $(`.toolpanel#select-panel .border-section #color-picker`).val();
            $(`.toolpanel#select-panel .border-section .color-display`).css("background-color", colorHex);

            _self.canvas.getActiveObjects().forEach(obj => obj.set('stroke', colorHex));
            _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
        })

        //$(`${this.containerSelector} .toolpanel#select-panel .border-section .colors .color`).click(function () {
        //    let colorBtn = $(this);

        //    $(`.toolpanel#select-panel .border-section .colors .color`).removeClass("is-selected");

        //    if (!colorBtn.hasClass("reset")) {
        //        colorBtn.addClass("is-selected");
        //    }

        //    $(`.toolpanel#select-panel .border-section #color-picker`).val(colorBtn.data("color"));

        //    let colorHex = $(`.toolpanel#select-panel .border-section #color-picker`).val();
        //    _self.canvas.getActiveObjects().forEach(obj => obj.set('stroke', colorHex));
        //    _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
        //})

        $(`${this.containerSelector} .toolpanel#select-panel .border-section #color-display-toggle`).on("click", function () {

            let icon = $(this).find("i");
            if (icon.hasClass("fa-angle-down")) {
                icon.removeClass("fa-angle-down");
                icon.addClass("fa-angle-up");
            }
            else {
                icon.addClass("fa-angle-down");
                icon.removeClass("fa-angle-up");
            }

            $(`.toolpanel#select-panel .border-section .color-panel`).toggleClass("is-opened");         
        })

        $("body").on("click", `${this.containerSelector} .toolpanel#select-panel .border-section .colors .color`, function () {
            let colorBtn = $(this);

            $(`.toolpanel#select-panel .border-section .colors .color`).removeClass("is-selected");

            colorBtn.addClass("is-selected");

            $(`.toolpanel#select-panel .border-section #color-picker`).val(colorBtn.data("color"));
            
            let colorHex = $(`.toolpanel#select-panel .border-section #color-picker`).val();

            $(`.toolpanel#select-panel .border-section .color-display`).css("background-color", colorHex);

            _self.canvas.getActiveObjects().forEach(obj => obj.set('stroke', colorHex));
            _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
        })

        $(`${this.containerSelector} .toolpanel#select-panel .border-section .color-control-panel .reset`).on("click", function () {
            let colorBtn = $(this);

            $(`.toolpanel#select-panel .border-section .colors .color`).removeClass("is-selected");

            $(`.toolpanel#select-panel .border-section #color-picker`).val(colorBtn.data("color"));

            let colorHex = $(`.toolpanel#select-panel .border-section #color-picker`).val();

            $(`.toolpanel#select-panel .border-section .color-display`).css("background-color", colorHex);

            _self.canvas.getActiveObjects().forEach(obj => obj.set('stroke', colorHex));
            _self.canvas.renderAll(), _self.canvas.trigger('object:modified');
        })

        $(`${this.containerSelector} .toolpanel#select-panel .border-section .color-control-panel #custom-color-mixer`).on("click", function () {
            let colorBtn = $(this);

            //colorBtn.addClass("hidden");
            //$(`.toolpanel#select-panel .border-section #custom-color-save`).removeClass("hidden");

            const colorPicker = $(`.toolpanel#select-panel .border-section #custom-color-picker`);
            let currentColor = $(`.toolpanel#select-panel .border-section #color-picker`).val();

            colorPicker.val(currentColor);

            colorPicker.trigger("click");
        })

        $(`${this.containerSelector} .toolpanel#select-panel .border-section .color-control-panel #custom-color-save`).on("click", function () {
            let colorBtn = $(this);
            //colorBtn.addClass("hidden");

            let currentColor = $(`.toolpanel#select-panel .border-section #color-picker`).val();

          
            let existedColor = false;
            $(`.toolpanel#select-panel .border-section .colors .color`).each(function() {
                if ($(this).data("color") === currentColor) {
                    existedColor = true;
                }
            })

            if (!existedColor) {
                $(`.toolpanel#select-panel .border-section .colors .color`).removeClass("is-selected");
                $(`.toolpanel#select-panel .border-section .colors`).append(`<button type="button" class="color regular-color main is-selected" data-color="${currentColor}" style="background:${currentColor}"></button>`);
            }
            

            //$(`.toolpanel#select-panel .border-section .color-control-panel #custom-color-mixer`).removeClass("hidden");
        })


      $(`${this.containerSelector} .toolpanel#select-panel .border-section #input-border-width`).change(function () {
        let width = parseInt($(this).val());
        _self.canvas.getActiveObjects().forEach(obj => obj.set({
          strokeUniform: true,
          strokeWidth: width
        }))
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      })

      $(`${this.containerSelector} .toolpanel#select-panel .border-section #input-border-style`).change(function () {
        try {
          let style = JSON.parse($(this).val());
          _self.canvas.getActiveObjects().forEach(obj => obj.set({
            strokeUniform: true,
            strokeDashArray: style.strokeDashArray,
            strokeLineCap: style.strokeLineCap
          }))
          _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
        } catch (_) {}
      })

      $(`${this.containerSelector} .toolpanel#select-panel .border-section #input-corner-type`).change(function () {
        let corner = $(this).val();
        _self.canvas.getActiveObjects().forEach(obj => obj.set('strokeLineJoin', corner))
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      })
    })();
    // end border section

    // fill color section
      var panelPropFillColor = {
          ColorFill: "塗りつぶしの色",
          GradientFill: "塗りつぶしのグラデーション",
          CustomColorPicker: "custom color"
      };

      (() => {

      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="fill-section">
          <div class="tab-container">
          <div class="tabs">
            <div class="tab-label" data-value="color-fill">${panelPropFillColor.ColorFill}</div>
            <div class="tab-label hidden" data-value="gradient-fill">${panelPropFillColor.GradientFill}</div>
          </div>
          <div class="tab-content" data-value="color-fill">
            <div class="color-display-wrapper">
                <div class="color-display" data-color="${mainColors.color1}" style="background:${mainColors.color1}">currentcolor</div>
                <button type="button" class="color-display-toggle" id="color-display-toggle"><i class="fa fa-angle-down" aria-hidden="true"></i></i></button>
            </div>
            <div class="color-panel">
                <div class="color-panel-content">
                    <input id="color-picker" value='transparent' class="hidden"/><br>
                    <div class="colors">
                        <button type="button" class="color regular-color" data-color="${mainColors.color1}" style="background:${mainColors.color1}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color2}" style="background:${mainColors.color2}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color3}" style="background:${mainColors.color3}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color4}" style="background:${mainColors.color4}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color5}" style="background:${mainColors.color5}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color6}" style="background:${mainColors.color6}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color7}" style="background:${mainColors.color7}"></button>
                        <button type="button" class="color regular-color" data-color="${mainColors.color8}" style="background:${mainColors.color8}"></button>           
                    </div>
                    <div class="color-control-panel">
                        <button type="button" class="color-ctrl edit" id="custom-color-mixer"><i class="fa fa-tint" aria-hidden="true"></i></button>
                        <button type="button" class="color-ctrl save" id="custom-color-save"><i class="fa fa-plus" aria-hidden="true"></i></button>
                        <button type="button" class="color-ctrl reset" id="custom-color-reset" data-color="black"><i class="fa fa-undo" aria-hidden="true"></i></button>
                        <input id="custom-color-picker" type="color" class="custom-color-picker"/>
                    </div>
                </div>
            </div>           
          </div>
          <div class="tab-content hidden" data-value="gradient-fill">
            <div id="gradient-picker"></div>
            <div class="gradient-orientation-container">
              <div class="input-container">
                <label>Orientation</label>
                <select id="select-orientation">
                  <option value="linear">Linear</option>
                  <option value="radial">Radial</option>
                </select>
              </div>
              <div id="angle-input-container" class="input-container">
                <label>Angle</label>
                <div class="custom-number-input">
                  <button type="button" class="decrease">-</button>
                  <input type="number" min="0" max="360" value="0" id="input-angle">
                  <button type="button" class="increase">+</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        </div>
      `);

          $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] #color-display-toggle`).on("click", function () {

              let icon = $(this).find("i");
              if (icon.hasClass("fa-angle-down")) {
                  icon.removeClass("fa-angle-down");
                  icon.addClass("fa-angle-up");
              }
              else {
                  icon.addClass("fa-angle-down");
                  icon.removeClass("fa-angle-up");
              }

              $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] .color-panel`).toggleClass("is-opened");
          })

        $("body").on("click", `${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`, function () {
            let colorBtn = $(this);

            $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`).removeClass("is-selected");

            colorBtn.addClass("is-selected");

            $(`${_self.containerSelector} .toolpanel#select-panel .fill-section #color-picker`).val(colorBtn.data('color'));
            $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-label[data-value=color-fill]`).click();
        })

        $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] #custom-color-picker`).on("input", function () {
            let colorBtn = $(this);

            $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`).removeClass("is-selected");

            $(`${_self.containerSelector} .toolpanel#select-panel .fill-section #color-picker`).val(colorBtn.val());
            $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-label[data-value=color-fill]`).click();
        })

          $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .color-control-panel #custom-color-mixer`).on("click", function () {
              //let colorBtn = $(this);

              //colorBtn.addClass("hidden");
              //$(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] #custom-color-save`).removeClass("hidden");

              const colorPicker = $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] #custom-color-picker`);
              let currentColor = $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] #color-picker`).val();

              colorPicker.val(currentColor);

              colorPicker.trigger("click");
          })

          $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .color-control-panel #custom-color-save`).on("click", function () {
              //let colorBtn = $(this);
              //colorBtn.addClass("hidden");

              let currentColor = $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] #color-picker`).val();

              let existedColor = false;
              $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`).each(function () {
                  if ($(this).data("color") === currentColor) {
                      existedColor = true;
                  }
              })

              if (!existedColor) {
                  $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`).removeClass("is-selected");

                  $(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors`).append(`<button type="button" class="color regular-color main is-selected" data-color="${currentColor}" style="background:${currentColor}"></button>`);
              }
             

              //$(`.toolpanel#select-panel .content .tab-content[data-value=color-fill] .color-control-panel #custom-color-mixer`).removeClass("hidden");
          })

          $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] #custom-color-reset`).on("click", function () {
              let colorBtn = $(this);

              $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-content[data-value=color-fill] .colors .color`).removeClass("is-selected");

              $(`${_self.containerSelector} .toolpanel#select-panel .fill-section #color-picker`).val(colorBtn.val());

              $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-label[data-value=color-fill]`).click();
          })

      $(`${this.containerSelector} .toolpanel#select-panel .content .tab-label`).click(function () {
        $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-label`).removeClass('active');
        $(this).addClass('active');
        let target = $(this).data('value');
        $(this).closest('.tab-container').find('.tab-content').hide();
        $(this).closest('.tab-container').find(`.tab-content[data-value=${target}]`).show();
        if (target === 'color-fill') {
          let color = $(`${_self.containerSelector} .toolpanel#select-panel .fill-section #color-picker`).val();
          try {
              $(`${_self.containerSelector} .toolpanel#select-panel .fill-section .color-display`).css("background-color", color);
            _self.canvas.getActiveObjects().forEach(obj => obj.set('fill', color))
            _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
          } catch (_) {
            console.log("can't update background color")
          }
        } else {
          updateGradientFill();
        }
      })

    
      $(`${_self.containerSelector} .toolpanel#select-panel .content .tab-label[data-value=color-fill]`).click();


      //$(`${this.containerSelector} .toolpanel#select-panel .fill-section #color-picker`).spectrum({
      //  flat: true,
      //  showPalette: false,
      //  showButtons: false,
      //  type: "color",
      //  showInput: "true",
      //  allowEmpty: "false",
      //  move: function (color) {
      //    let hex = 'transparent';
      //    color && (hex = color.toRgbString()); // #ff0000
      //    _self.canvas.getActiveObjects().forEach(obj => obj.set('fill', hex))
      //    _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      //  }
      //});

      const gp = new Grapick({
        el: `${this.containerSelector} .toolpanel#select-panel .fill-section #gradient-picker`,
        colorEl: '<input id="colorpicker"/>'
      });

      gp.setColorPicker(handler => {
        const el = handler.getEl().querySelector('#colorpicker');
        $(el).spectrum({
          showPalette: false,
          showButtons: false,
          type: "color",
          color: handler.getColor(),
          showAlpha: true,
          change(color) {
            handler.setColor(color.toRgbString());
          },
          move(color) {
            handler.setColor(color.toRgbString(), 0);
          }
        });
      });
      gp.addHandler(0, 'red');
      gp.addHandler(100, 'blue');

      const updateGradientFill = () => {
        let stops = gp.getHandlers();
        let orientation = $(`${this.containerSelector} .toolpanel#select-panel .content .gradient-orientation-container #select-orientation`).val();
        let angle = parseInt($(`${this.containerSelector} .toolpanel#select-panel .content .gradient-orientation-container #input-angle`).val());

        let gradient = generateFabricGradientFromColorStops(stops, _self.activeSelection.width, _self.activeSelection.height, orientation, angle);
        _self.activeSelection.set('fill', gradient);
        _self.canvas.renderAll()
      }

      gp.on('change', complete => {
        updateGradientFill();
      })

      $(`${this.containerSelector} .toolpanel#select-panel .content .gradient-orientation-container #select-orientation`).change(function () {
        let type = $(this).val();
        console.log('orientation', type)
        if (type === 'radial') {
          $(this).closest('.gradient-orientation-container').find('#angle-input-container').hide();
        } else {
          $(this).closest('.gradient-orientation-container').find('#angle-input-container').show();
        }
        updateGradientFill();
      })

      $(`${this.containerSelector} .toolpanel#select-panel .content .gradient-orientation-container #input-angle`).change(function () {
        updateGradientFill();
      })

    })();
    // end fill color section

    // alignment section
      var panelPropAlignment = {
          Alignment: "配置"
      };

    (() => {
      let buttons = ``;
      AlignmentButtonList.forEach(item => {
          buttons += `<button type="button" data-tooltip="${item.desc}" data-pos="${item.pos}">${item.icon}</button>`
      })
      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="alignment-section">
          <h4>${panelPropAlignment.Alignment}</h4>
          <div class="meo-btn-box">${buttons}</div>
          <hr>
        </div>
      `);

      $(`${this.containerSelector} .toolpanel#select-panel .alignment-section button`).click(function () {
        let pos = $(this).data('pos');
        alignObject(_self.canvas, _self.activeSelection, pos);
      })
    })();
    // end alignment section

    // object options section
    (() => {
        var panelPropObject = {
            ObjectOptions: "オブジェクト",
        };

      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="object-options">
          <h4>${panelPropObject.ObjectOptions}</h4>
          <button type="button" id="flip-h"><svg width="512" height="512" enable-background="new 0 0 16 16" viewBox="0 0 16 20" xml:space="preserve"><g transform="matrix(0 1.5365 1.5385 0 -5.0769 1.5495)"><rect x="5" y="8" width="1" height="1"></rect><rect x="7" y="8" width="1" height="1"></rect><rect x="9" y="8" width="1" height="1"></rect><rect x="1" y="8" width="1" height="1"></rect><rect x="3" y="8" width="1" height="1"></rect><path d="M 1,2 5.5,6 10,2 Z M 7.37,3 5.5,4.662 3.63,3 Z"></path><polygon points="10 15 5.5 11 1 15"></polygon></g></svg></button>
          <button type="button" id="flip-v"><svg width="512" height="512" enable-background="new 0 0 16 16" viewBox="0 0 16 20" xml:space="preserve"><g transform="matrix(1.5365 0 0 1.5385 -.45052 -3.0769)"><rect x="5" y="8" width="1" height="1"></rect><rect x="7" y="8" width="1" height="1"></rect><rect x="9" y="8" width="1" height="1"></rect><rect x="1" y="8" width="1" height="1"></rect><rect x="3" y="8" width="1" height="1"></rect><path d="M 1,2 5.5,6 10,2 Z M 7.37,3 5.5,4.662 3.63,3 Z"></path><polygon points="5.5 11 1 15 10 15"></polygon></g></svg></button>
          <button type="button" id="bring-fwd"><svg x="0px" y="0px" viewBox="0 0 1000 1000" enable-background="new 0 0 1000 1000" xml:space="preserve"><g><path d="M10,10h686v686H10V10 M990,304v686H304V794h98v98h490V402h-98v-98H990z"></path></g></svg></button>
          <button type="button" id="bring-back"><svg enable-background="new 0 0 1000 1000" viewBox="0 0 1e3 1e3" xml:space="preserve"><path d="m990 990h-686v-686h686v686m-980-294v-686h686v680h-98v-582h-490v490h200v98z"></path><rect x="108.44" y="108" width="490" height="490" fill="#fff"></rect></svg></button>
          <button type="button" id="duplicate"><svg id="Capa_1" x="0px" y="0px" viewBox="0 0 512 512" xml:space="preserve"><g><g><g><path d="M42.667,256c0-59.52,35.093-110.827,85.547-134.827V75.2C53.653,101.44,0,172.48,0,256s53.653,154.56,128.213,180.8 v-45.973C77.76,366.827,42.667,315.52,42.667,256z"></path><path d="M320,64c-105.92,0-192,86.08-192,192s86.08,192,192,192s192-86.08,192-192S425.92,64,320,64z M320,405.333 c-82.347,0-149.333-66.987-149.333-149.333S237.653,106.667,320,106.667S469.333,173.653,469.333,256 S402.347,405.333,320,405.333z"></path><polygon points="341.333,170.667 298.667,170.667 298.667,234.667 234.667,234.667 234.667,277.333 298.667,277.333 298.667,341.333 341.333,341.333 341.333,277.333 405.333,277.333 405.333,234.667 341.333,234.667  "></polygon></g></g></g></svg></button>
          <button type="button" id="delete"><svg id="Layer_1" x="0px" y="0px" viewBox="0 0 512 512" xml:space="preserve"><g><g><path d="M425.298,51.358h-91.455V16.696c0-9.22-7.475-16.696-16.696-16.696H194.855c-9.22,0-16.696,7.475-16.696,16.696v34.662 H86.704c-9.22,0-16.696,7.475-16.696,16.696v51.357c0,9.22,7.475,16.696,16.696,16.696h5.072l15.26,359.906 c0.378,8.937,7.735,15.988,16.68,15.988h264.568c8.946,0,16.302-7.051,16.68-15.989l15.259-359.906h5.073 c9.22,0,16.696-7.475,16.696-16.696V68.054C441.994,58.832,434.519,51.358,425.298,51.358z M211.551,33.391h88.9v17.967h-88.9 V33.391z M372.283,478.609H139.719l-14.522-342.502h261.606L372.283,478.609z M408.602,102.715c-15.17,0-296.114,0-305.202,0 V84.749h305.202V102.715z"></path></g></g><g><g><path d="M188.835,187.304c-9.22,0-16.696,7.475-16.696,16.696v206.714c0,9.22,7.475,16.696,16.696,16.696 c9.22,0,16.696-7.475,16.696-16.696V204C205.53,194.779,198.055,187.304,188.835,187.304z"></path></g></g><g><g><path d="M255.998,187.304c-9.22,0-16.696,7.475-16.696,16.696v206.714c0,9.22,7.474,16.696,16.696,16.696 c9.22,0,16.696-7.475,16.696-16.696V204C272.693,194.779,265.218,187.304,255.998,187.304z"></path></g></g><g><g><path d="M323.161,187.304c-9.22,0-16.696,7.475-16.696,16.696v206.714c0,9.22,7.475,16.696,16.696,16.696 s16.696-7.475,16.696-16.696V204C339.857,194.779,332.382,187.304,323.161,187.304z"></path></g></g></svg></button>
          <button type="button" id="group"><svg width="248" height="249" viewBox="0 0 248 249"><g><rect fill="none" id="canvas_background" height="251" width="250" y="-1" x="-1"></rect><g display="none" overflow="visible" y="0" x="0" height="100%" width="100%" id="canvasGrid"><rect fill="url(#gridpattern)" stroke-width="0" y="0" x="0" height="100%" width="100%"></rect></g></g><g><rect id="svg_1" height="213.999997" width="213.999997" y="18.040149" x="16.8611" stroke-width="14" stroke="#000" fill="none"></rect><ellipse ry="39.5" rx="39.5" id="svg_2" cy="87.605177" cx="90.239139" stroke-opacity="null" stroke-width="5" stroke="#000" fill="#000000"></ellipse><rect id="svg_3" height="61.636373" width="61.636373" y="135.606293" x="133.750604" stroke-opacity="null" stroke-width="5" stroke="#000" fill="#000000"></rect><rect id="svg_4" height="26.016205" width="26.016205" y="4.813006" x="3.999997" stroke-opacity="null" stroke-width="8" stroke="#000" fill="#000000"></rect><rect id="svg_5" height="26.016205" width="26.016205" y="3.999999" x="217.820703" stroke-opacity="null" stroke-width="8" stroke="#000" fill="#000000"></rect><rect id="svg_7" height="26.016205" width="26.016205" y="218.633712" x="3.999997" stroke-opacity="null" stroke-width="8" stroke="#000" fill="#000000"></rect><rect id="svg_8" height="26.016205" width="26.016205" y="218.633712" x="217.820694" stroke-opacity="null" stroke-width="8" stroke="#000" fill="#000000"></rect></g></svg></button>
          <button type="button" id="ungroup"><svg width="247.99999999999997" height="248.99999999999997" viewBox="0 0 248 249"><g><rect fill="none" id="canvas_background" height="251" width="250" y="-1" x="-1"></rect><g display="none" overflow="visible" y="0" x="0" height="100%" width="100%" id="canvasGrid"><rect fill="url(#gridpattern)" stroke-width="0" y="0" x="0" height="100%" width="100%"></rect></g></g><g><rect stroke-dasharray="20" id="svg_1" height="213.999997" width="213.999997" y="18.040149" x="16.8611" stroke-width="16" stroke="#000" fill="none"></rect><ellipse ry="39.5" rx="39.5" id="svg_2" cy="87.605177" cx="90.239139" stroke-opacity="null" stroke-width="5" stroke="#000" fill="#000000"></ellipse><rect id="svg_3" height="61.636373" width="61.636373" y="135.606293" x="133.750604" stroke-opacity="null" stroke-width="5" stroke="#000" fill="#000000"></rect></g></svg></button>
          <hr>
        </div>
      `);

      $(`${this.containerSelector} .toolpanel#select-panel .object-options #flip-h`).click(() => {
        this.activeSelection.set('flipX', !this.activeSelection.flipX);
        this.canvas.renderAll(), this.canvas.trigger('object:modified');
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #flip-v`).click(() => {
        this.activeSelection.set('flipY', !this.activeSelection.flipY);
        this.canvas.renderAll(), this.canvas.trigger('object:modified');
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #bring-fwd`).click(() => {
        this.canvas.bringForward(this.activeSelection)
        this.canvas.renderAll(), this.canvas.trigger('object:modified');
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #bring-back`).click(() => {
        this.canvas.sendBackwards(this.activeSelection)
        this.canvas.renderAll(), this.canvas.trigger('object:modified');
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #duplicate`).click(() => {
        let clonedObjects = []
        let activeObjects = this.canvas.getActiveObjects()
        activeObjects.forEach(obj => {
          obj.clone(clone => {
            this.canvas.add(clone.set({
              strokeUniform: true,
              left: obj.aCoords.tl.x + 20,
              top: obj.aCoords.tl.y + 20
            }));

            if (activeObjects.length === 1) {
              this.canvas.setActiveObject(clone)
            }
            clonedObjects.push(clone)
          })
        })

        if (clonedObjects.length > 1) {
          let sel = new fabric.ActiveSelection(clonedObjects, {
            canvas: this.canvas,
          });
          this.canvas.setActiveObject(sel)
        }

        this.canvas.requestRenderAll(), this.canvas.trigger('object:modified')
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #delete`).click(() => {
        this.canvas.getActiveObjects().forEach(obj => this.canvas.remove(obj))
        this.canvas.discardActiveObject().requestRenderAll(), this.canvas.trigger('object:modified');
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #group`).click(() => {
        if (this.activeSelection.type !== 'activeSelection') return;
        this.canvas.getActiveObject().toGroup()
        this.canvas.requestRenderAll(), this.canvas.trigger('object:modified')
      })
      $(`${this.containerSelector} .toolpanel#select-panel .object-options #ungroup`).click(() => {
        if (this.activeSelection.type !== 'group') return;
        this.canvas.getActiveObject().toActiveSelection()
        this.canvas.requestRenderAll(), this.canvas.trigger('object:modified');
      })
    })();
    // end object options section

    // effect section
      (() => {
          var panelPropEffect = {
              Effect: "効果",
              Opacity: "不透明度",
              Blur: "ぼかしさ",
              Brightness: "明るさ",
              Saturation: "飽和度",
              Gamma: "ガンマ",
              Red: "赤色",
              Green: "緑色",
              Blue: "青色"
          }

      $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
        <div class="effect-section">
          <h4>${panelPropEffect.Effect}</h4>
          <div class="input-container"><label>${panelPropEffect.Opacity}</label><input id="opacity" type="range" min="0" max="1" value="1" step="0.01"></div>
          <div class="input-container"><label>${panelPropEffect.Blur}</label><input class="effect" id="blur" type="range" min="0" max="100" value="50"></div>
          <div class="input-container"><label>${panelPropEffect.Brightness}</label><input class="effect" id="brightness" type="range" min="0" max="100" value="50"></div>
          <div class="input-container"><label>${panelPropEffect.Saturation}</label><input class="effect" id="saturation" type="range" min="0" max="100" value="50"></div>
          <h4 class="mt-3">${panelPropEffect.Gamma}</h4>
          <div class="input-container"><label>${panelPropEffect.Red}</label><input class="effect" id="gamma.r" type="range" min="0" max="100" value="50"></div>
          <div class="input-container"><label>${panelPropEffect.Green}</label><input class="effect" id="gamma.g" type="range" min="0" max="100" value="50"></div>
          <div class="input-container"><label>${panelPropEffect.Blue}</label><input class="effect" id="gamma.b" type="range" min="0" max="100" value="50"></div>
          <hr>
        </div>
      `);

      //  $(`${this.containerSelector} .toolpanel#select-panel .content`).append(`
      //  <div class="effect-section">
      //    <h4>Effect</h4>
      //    <div class="input-container"><label>Opacity</label><input id="opacity" type="range" min="0" max="1" value="1" step="0.01"></div>
      //    <div class="input-container"><label>Blur</label><input class="effect" id="blur" type="range" min="0" max="100" value="50"></div>
      //    <div class="input-container"><label>Brightness</label><input class="effect" id="brightness" type="range" min="0" max="100" value="50"></div>
      //    <div class="input-container"><label>Saturation</label><input class="effect" id="saturation" type="range" min="0" max="100" value="50"></div>         
      //  </div>
      //`);

      $(`${this.containerSelector} .toolpanel#select-panel .effect-section #opacity`).change(function () {
        let opacity = parseFloat($(this).val());
        _self.activeSelection.set('opacity', opacity)
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      })

      $(`${this.containerSelector} .toolpanel#select-panel .effect-section .effect`).change(function () {
        let effect = $(this).attr('id');
        let value = parseFloat($(this).val());
        let currentEffect = getCurrentEffect(_self.activeSelection);
        _self.activeSelection.filters = getUpdatedFilter(currentEffect, effect, value);
        _self.activeSelection.applyFilters();
        _self.canvas.renderAll(), _self.canvas.trigger('object:modified')
      })
    })();
    // end effect section
  }

  window.ImageEditor.prototype.initializeSelectionSettings = selectionSettings;
})()
var Jcrop = function (e) {
    var t = {};

    function n(r) {
        if (t[r]) return t[r].exports;
        var i = t[r] = {
            i: r,
            l: !1,
            exports: {}
        };
        return e[r].call(i.exports, i, i.exports, n), i.l = !0, i.exports
    }
    return n.m = e, n.c = t, n.d = function (e, t, r) {
        n.o(e, t) || Object.defineProperty(e, t, {
            enumerable: !0,
            get: r
        })
    }, n.r = function (e) {
        "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, {
            value: "Module"
        }), Object.defineProperty(e, "__esModule", {
            value: !0
        })
    }, n.t = function (e, t) {
        if (1 & t && (e = n(e)), 8 & t) return e;
        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
        var r = Object.create(null);
        if (n.r(r), Object.defineProperty(r, "default", {
            enumerable: !0,
            value: e
        }), 2 & t && "string" != typeof e)
            for (var i in e) n.d(r, i, function (t) {
                return e[t]
            }.bind(null, i));
        return r
    }, n.n = function (e) {
        var t = e && e.__esModule ? function () {
            return e.default
        } : function () {
            return e
        };
        return n.d(t, "a", t), t
    }, n.o = function (e, t) {
        return Object.prototype.hasOwnProperty.call(e, t)
    }, n.p = "", n(n.s = 12)
}([function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    }), t.default = function () {
        var e = {};
        for (var t in arguments) {
            var n = arguments[t];
            for (var r in n) Object.prototype.hasOwnProperty.call(n, r) && (e[r] = n[r])
        }
        return e
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }();
    var i = function () {
        function e(t) {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, e), "string" == typeof t && (t = document.getElementById(t)), this.el = t
        }
        return r(e, [{
            key: "appendTo",
            value: function (e) {
                return "string" == typeof e && (e = document.getElementById(e)), e.appendChild(this.el), this
            }
        }, {
            key: "emit",
            value: function (e) {
                var t = document.createEvent("Event");
                t.initEvent(e, !0, !0), t.cropTarget = this, this.el.dispatchEvent(t)
            }
        }, {
            key: "removeClass",
            value: function (e) {
                return this.el.className = this.el.className.split(" ").filter(function (t) {
                    return e !== t
                }).join(" "), this
            }
        }, {
            key: "hasClass",
            value: function (e) {
                return this.el.className.split(" ").filter(function (t) {
                    return e === t
                }).length
            }
        }, {
            key: "addClass",
            value: function (e) {
                return this.hasClass(e) || (this.el.className += " " + e), this
            }
        }, {
            key: "listen",
            value: function (e, t) {
                return this.el.addEventListener(e, function (e) {
                    return t(e.cropTarget, e)
                }), this
            }
        }]), e
    }();
    t.default = i
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }();
    var i = function () {
        function e() {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, e), this.x = 0, this.y = 0, this.w = 0, this.h = 0
        }
        return r(e, [{
            key: "round",
            value: function () {
                return e.create(Math.round(this.x), Math.round(this.y), Math.round(this.w), Math.round(this.h))
            }
        }, {
            key: "normalize",
            value: function () {
                var t = [Math.min(this.x, this.x2), Math.min(this.y, this.y2), Math.max(this.x, this.x2), Math.max(this.y, this.y2)],
                    n = t[0],
                    r = t[1],
                    i = t[2],
                    o = t[3];
                return e.create(n, r, i - n, o - r)
            }
        }, {
            key: "rebound",
            value: function (e, t) {
                var n = this.normalize();
                return n.x < 0 && (n.x = 0), n.y < 0 && (n.y = 0), n.x2 > e && (n.x = e - n.w), n.y2 > t && (n.y = t - n.h), n
            }
        }, {
            key: "scale",
            value: function (t, n) {
                return n = n || t, e.create(this.x, this.y, this.w * t, this.h * n)
            }
        }, {
            key: "center",
            value: function (t, n) {
                return e.create((t - this.w) / 2, (n - this.h) / 2, this.w, this.h)
            }
        }, {
            key: "toArray",
            value: function () {
                return [this.x, this.y, this.w, this.h]
            }
        }, {
            key: "x1",
            set: function (e) {
                this.w = this.x2 - e, this.x = e
            }
        }, {
            key: "y1",
            set: function (e) {
                this.h = this.y2 - e, this.y = e
            }
        }, {
            key: "x2",
            get: function () {
                return this.x + this.w
            },
            set: function (e) {
                this.w = e - this.x
            }
        }, {
            key: "y2",
            get: function () {
                return this.y + this.h
            },
            set: function (e) {
                this.h = e - this.y
            }
        }, {
            key: "aspect",
            get: function () {
                return this.w / this.h
            }
        }]), e
    }();
    i.fromPoints = function (e, t) {
        var n = [Math.min(e[0], t[0]), Math.min(e[1], t[1]), Math.max(e[0], t[0]), Math.max(e[1], t[1])],
            r = n[0],
            o = n[1],
            a = n[2],
            u = n[3];
        return i.create(r, o, a - r, u - o)
    }, i.create = function () {
        var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : 0,
            t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : 0,
            n = arguments.length > 2 && void 0 !== arguments[2] ? arguments[2] : 0,
            r = arguments.length > 3 && void 0 !== arguments[3] ? arguments[3] : 0,
            o = new i;
        return o.x = e, o.y = t, o.w = n, o.h = r, o
    }, i.from = function (e) {
        if (Array.isArray(e)) return i.fromArray(e);
        var t = new i;
        return t.x = e.offsetLeft, t.y = e.offsetTop, t.w = e.offsetWidth, t.h = e.offsetHeight, t
    }, i.fromArray = function (e) {
        if (4 === e.length) return i.create.apply(this, e);
        if (2 === e.length) return i.fromPoints(e[0], e[1]);
        throw "fromArray method problem"
    }, i.sizeOf = function (e, t) {
        if (t) return i.create(0, 0, e, t);
        var n = new i;
        return n.w = e.offsetWidth, n.h = e.offsetHeight, n
    }, i.getMax = function (e, t, n) {
        return e / t > n ? [t * n, t] : [e, e / n]
    }, i.fromPoint = function (e, t, n) {
        var r = arguments.length > 3 && void 0 !== arguments[3] ? arguments[3] : "br",
            o = new i;
        switch (o.x = e[0], o.y = e[1], r) {
            case "br":
                o.x2 = o.x + t, o.y2 = o.y + n;
                break;
            case "bl":
                o.x2 = o.x - t, o.y2 = o.y + n;
                break;
            case "tl":
                o.x2 = o.x - t, o.y2 = o.y - n;
                break;
            case "tr":
                o.x2 = o.x + t, o.y2 = o.y - n
        }
        return o
    }, t.default = i
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    }), t.default = {
        uniqueId: "",
        animateEasingFunction: "swing",
        animateFrames: 30,
        multi: !1,
        multiMax: null,
        multiMin: 1,
        cropperClass: "jcrop-widget",
        disabledClass: "jcrop-disable",
        canDrag: !0,
        canResize: !0,
        canSelect: !0,
        canRemove: !0,
        multiple: 1,
        autoFront: !0,
        active: !0,
        handles: ["n", "s", "e", "w", "sw", "nw", "ne", "se"],
        shade: !0,
        shadeClass: "jcrop-shade",
        shadeColor: "black",
        shadeOpacity: 0.5,
        widgetConstructor: null,
        x: 0,
        y: 0,
        w: 100,
        h: 100,
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    t.default = function (e, t, n, r) {
        var i, o;

        function a(e) {
            var n = "touchstart" === e.type ? e.touches[0] : e;
            i = n.pageX, o = n.pageY, e.preventDefault(), e.stopPropagation(), t(i, o, n) && ("mousedown" === e.type ? (window.addEventListener("mousemove", u), document.addEventListener("mouseup", s)) : "touchstart" === e.type && (document.addEventListener("touchmove", u), document.addEventListener("touchend", s)))
        }

        function u(e) {
            var t = "touchmove" === e.type ? e.changedTouches[0] : e;
            e.stopPropagation(), n(t.pageX - i, t.pageY - o)
        }

        function s(e) {
            var t = "touchend" === e.type ? e.changedTouches[0] : e;
            t.pageX && t.pageY && n(t.pageX - i, t.pageY - o), document.removeEventListener("mouseup", s), window.removeEventListener("mousemove", u), document.removeEventListener("touchmove", u), document.removeEventListener("touchend", s), r()
        }
        return "string" == typeof e && (e = document.getElementById(e)), e.addEventListener("mousedown", a), e.addEventListener("touchstart", a), {
            remove: function () {
                e.removeEventListener("mousedown", a), e.removeEventListener("touchstart", a)
            }
        }
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        return function (e, t) {
            if (Array.isArray(e)) return e;
            if (Symbol.iterator in Object(e)) return function (e, t) {
                var n = [],
                    r = !0,
                    i = !1,
                    o = void 0;
                try {
                    for (var a, u = e[Symbol.iterator](); !(r = (a = u.next()).done) && (n.push(a.value), !t || n.length !== t); r = !0);
                } catch (e) {
                    i = !0, o = e
                } finally {
                    try {
                        !r && u.return && u.return()
                    } finally {
                        if (i) throw o
                    }
                }
                return n
            }(e, t);
            throw new TypeError("Invalid attempt to destructure non-iterable instance")
        }
    }(),
        i = function () {
            function e(e, t) {
                for (var n = 0; n < t.length; n++) {
                    var r = t[n];
                    r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
                }
            }
            return function (t, n, r) {
                return n && e(t.prototype, n), r && e(t, r), t
            }
        }(),
        o = function (e) {
            return e && e.__esModule ? e : {
                default: e
            }
        }(n(2));
    var a = function () {
        function e(t, n, r, i) {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, e), this.sw = n, this.sh = r, this.rect = t, this.locked = this.getCornerPoint(this.getOppositeCorner(i)), this.stuck = this.getCornerPoint(i)
        }
        return i(e, [{
            key: "move",
            value: function (e, t) {
                return o.default.fromPoints(this.locked, this.translateStuckPoint(e, t))
            }
        }, {
            key: "getDragQuadrant",
            value: function (e, t) {
                var n = this.locked[0] - e,
                    r = this.locked[1] - t;
                return n < 0 && r < 0 ? "br" : n >= 0 && r >= 0 ? "tl" : n < 0 && r >= 0 ? "tr" : "bl"
            }
        }, {
            key: "getMaxRect",
            value: function (e, t, n) {
                return o.default.getMax(Math.abs(this.locked[0] - e), Math.abs(this.locked[1] - t), n)
            }
        }, {
            key: "translateStuckPoint",
            value: function (e, t) {
                var n = r(this.stuck, 3),
                    i = n[0],
                    a = n[1],
                    u = n[2],
                    s = null === i ? u : i + e,
                    c = null === a ? u : a + t;
                if (s > this.sw && (s = this.sw), c > this.sh && (c = this.sh), s < 0 && (s = 0), c < 0 && (c = 0), this.aspect) {
                    var f = this.getMaxRect(s, c, this.aspect),
                        l = r(f, 2),
                        h = l[0],
                        d = l[1],
                        p = this.getDragQuadrant(s, c),
                        v = o.default.fromPoint(this.locked, h, d, p);
                    return [v.x2, v.y2]
                }
                return [s, c]
            }
        }, {
            key: "getCornerPoint",
            value: function (e) {
                var t = this.rect;
                switch (e) {
                    case "n":
                        return [null, t.y, t.x];
                    case "s":
                        return [null, t.y2, t.x2];
                    case "e":
                        return [t.x2, null, t.y2];
                    case "w":
                        return [t.x, null, t.y];
                    case "se":
                        return [t.x2, t.y2];
                    case "sw":
                        return [t.x, t.y2];
                    case "ne":
                        return [t.x2, t.y];
                    case "nw":
                        return [t.x, t.y]
                }
            }
        }, {
            key: "getOppositeCorner",
            value: function (e) {
                switch (e) {
                    case "n":
                        return "se";
                    case "s":
                    case "e":
                        return "nw";
                    case "w":
                        return "se";
                    case "se":
                        return "nw";
                    case "sw":
                        return "ne";
                    case "ne":
                        return "sw";
                    case "nw":
                        return "se"
                }
            }
        }]), e
    }();
    a.create = function (e, t, n, r) {
        return new a(e, t, n, r)
    }, t.default = a
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }(),
        i = f(n(0)),
        o = f(n(7)),
        a = f(n(11)),
        u = f(n(4)),
        s = f(n(9)),
        c = f(n(5));

    function f(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }
    var l = function (e) {
        function t(e, n) {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, t);
            var r = function (e, t) {
                if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                return !t || "object" != typeof t && "function" != typeof t ? e : t
            }(this, (t.__proto__ || Object.getPrototypeOf(t)).call(this, e, n));
            return r.crops = new Set, r.active = null, r.enabled = !0, r.init(), r
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, s.default), r(t, [{
            key: "init",
            value: function () {
                this.initStageDrag(), a.default.Manager.attach(this)
            }
        }, {
            key: "initOptions",
            value: function () {
                var e = this;
                this._optconf.multi = function (t) {
                    t || e.limitWidgets()
                }
            }
        }, {
            key: "setEnabled",
            value: function () {
                var e = !(arguments.length > 0 && void 0 !== arguments[0]) || arguments[0],
                    t = this.options.disabledClass || "jcrop-disable";
                return this[e ? "removeClass" : "addClass"](t), this.enabled = !!e, this
            }
        }, {
            key: "focus",
            value: function () {
                return !!this.enabled && (this.active ? this.active.el.focus() : this.el.focus(), this)
            }
        }, {
            key: "limitWidgets",
            value: function () {
                var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : 1;
                if (!this.crops || e < 1) return !1;
                for (var t = Array.from(this.crops); t.length > e;) this.removeWidget(t.shift());
                return this
            }
        }, {
            key: "canCreate",
            value: function () {
                var e = this.crops.size,
                    t = this.options;
                return !!this.enabled && (!(null !== t.multiMax && e >= t.multiMax) && !(!t.multi && e >= t.multiMin))
            }
        }, {
            key: "canRemove",
            value: function () {
                var e = this.crops.size,
                    t = this.options;
                return !!this.enabled && (!(this.active && !this.active.options.canRemove) && !(!t.canRemove || e <= t.multiMin))
            }
        }, {
            key: "initStageDrag",
            value: function () {
                var e, t, n, r, i, a = this;
                (0, u.default)(this.el, function (u, s, f) {
                    return !!a.canCreate() && (e = (a.options.widgetConstructor || o.default).create(a.options), (t = e.pos).x = f.pageX - a.el.offsetParent.offsetLeft - a.el.offsetLeft, t.y = f.pageY - a.el.offsetParent.offsetTop - a.el.offsetTop, n = a.el.offsetWidth, r = a.el.offsetHeight, a.addWidget(e), i = c.default.create(t, n, r, "se"), a.options.aspectRatio && (i.aspect = a.options.aspectRatio), e.render(t), a.focus(), !0)
                }, function (t, n) {
                    e.render(i.move(t, n))
                }, function () {
                    e.emit("crop.change")
                })
            }
        }, {
            key: "reorderWidgets",
            value: function () {
                var e = this,
                    t = 10;
                this.crops.forEach(function (n) {
                    n.el.style.zIndex = t++, e.active === n ? n.addClass("active") : n.removeClass("active")
                }), this.refresh()
            }
        }, {
            key: "activate",
            value: function (e) {
                if (!this.enabled) return this;
                if (e = e || Array.from(this.crops).pop()) {
                    if (this.active === e) return;
                    this.active = e, this.crops.delete(e), this.crops.add(e), this.reorderWidgets(), this.active.el.focus(), this.options.shade && this.shades.enable(), e.emit("crop.activate")
                } else this.shades.disable();
                return this
            }
        }, {
            key: "addWidget",
            value: function (e) {
                return e.attachToStage(this), e.appendTo(this.el), this.activate(e), this
            }
        }, {
            key: "newWidget",
            value: function (e) {
                var t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
                t = (0, i.default)({}, this.options, t);
                var n = (this.options.widgetConstructor || o.default).create(t);
                return n.render(e), this.addWidget(n), n.el.focus(), n
            }
        }, {
            key: "removeWidget",
            value: function (e) {
                if (!this.canRemove()) return !1;
                e.emit("crop.remove"), e.el.remove(), this.crops.delete(e), this.activate()
            }
        }, {
            key: "refresh",
            value: function () {
                this.options.shade && this.active && this.shades.adjust(this.active.pos)
            }
        }, {
            key: "updateShades",
            value: function () {
                if (this.shades) return this.options.shade ? this.shades.enable() : this.shades.disable(), this.options.shade && this.active && this.shades.adjust(this.active.pos), this
            }
        }, {
            key: "setOptions",
            value: function () {
                var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                (function e(t, n, r) {
                    null === t && (t = Function.prototype);
                    var i = Object.getOwnPropertyDescriptor(t, n);
                    if (void 0 === i) {
                        var o = Object.getPrototypeOf(t);
                        return null === o ? void 0 : e(o, n, r)
                    }
                    if ("value" in i) return i.value;
                    var a = i.get;
                    return void 0 !== a ? a.call(r) : void 0
                })(t.prototype.__proto__ || Object.getPrototypeOf(t.prototype), "setOptions", this).call(this, e), this.crops && Array.from(this.crops).forEach(function (t) {
                    return t.setOptions(e)
                })
            }
        }, {
            key: "destroy",
            value: function () { }
        }]), t
    }();
    t.default = l
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        return function (e, t) {
            if (Array.isArray(e)) return e;
            if (Symbol.iterator in Object(e)) return function (e, t) {
                var n = [],
                    r = !0,
                    i = !1,
                    o = void 0;
                try {
                    for (var a, u = e[Symbol.iterator](); !(r = (a = u.next()).done) && (n.push(a.value), !t || n.length !== t); r = !0);
                } catch (e) {
                    i = !0, o = e
                } finally {
                    try {
                        !r && u.return && u.return()
                    } finally {
                        if (i) throw o
                    }
                }
                return n
            }(e, t);
            throw new TypeError("Invalid attempt to destructure non-iterable instance")
        }
    }(),
        i = function () {
            function e(e, t) {
                for (var n = 0; n < t.length; n++) {
                    var r = t[n];
                    r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
                }
            }
            return function (t, n, r) {
                return n && e(t.prototype, n), r && e(t, r), t
            }
        }(),
        o = p(n(0)),
        a = p(n(8)),
        u = p(n(3)),
        s = p(n(4)),
        c = p(n(2)),
        f = p(n(5)),
        l = p(n(9)),
        h = p(n(13)),
        d = p(n(14));

    function p(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }
    var v = function (e) {
        function t(e) {
            var n = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, t);
            var r = function (e, t) {
                if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                return !t || "object" != typeof t && "function" != typeof t ? e : t
            }(this, (t.__proto__ || Object.getPrototypeOf(t)).call(this, e, n));
            return r.pos = c.default.from(r.el), r.init(), r
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, l.default), i(t, [{
            key: "init",
            value: function () {
                return this.createHandles(), this.createMover(), this.attachFocus(), h.default.attach(this), this
            }
        }, {
            key: "initOptions",
            value: function () {
                var e = this;
                this._optconf.aspectRatio = function (t) {
                    var n = e.pos;
                    if (e.aspect = t || null, e.aspect && n) {
                        var i = c.default.getMax(n.w, n.h, t),
                            o = r(i, 2),
                            a = o[0],
                            u = o[1];
                        e.render(c.default.fromPoint([n.x, n.y], a, u))
                    }
                }
            }
        }, {
            key: "attachToStage",
            value: function (e) {
                this.stage = e, this.emit("crop.attach")
            }
        }, {
            key: "attachFocus",
            value: function () {
                var e = this;
                this.el.addEventListener("focus", function (t) {
                    e.stage.activate(e), e.emit("crop.update")
                }, !1)
            }
        }, {
            key: "animate",
            value: function (e, t, n) {
                var r = this,
                    i = this;
                return n = n || i.options.animateEasingFunction || "swing", t = t || i.options.animateFrames || 30, (0, d.default)(i.el, i.pos, e, function (e) {
                    return i.render(e.normalize())
                }, t, n).then(function () {
                    return r.emit("crop.change")
                })
            }
        }, {
            key: "createMover",
            value: function () {
                var e, t, n, r = this;
                this.pos = c.default.from(this.el), (0, s.default)(this.el, function () {
                    var i = r.el.parentElement;
                    if (!r.stage.enabled) return !1;
                    var o = [i.offsetWidth, i.offsetHeight];
                    return e = o[0], t = o[1], n = c.default.from(r.el), r.el.focus(), r.stage.activate(r), !0
                }, function (i, o) {
                    r.pos.x = n.x + i, r.pos.y = n.y + o, r.render(r.pos.rebound(e, t))
                }, function () {
                    r.emit("crop.change")
                })
            }
        }, {
            key: "nudge",
            value: function () {
                var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : 0,
                    t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : 0,
                    n = this.el.parentElement,
                    r = [n.offsetWidth, n.offsetHeight],
                    i = r[0],
                    o = r[1];
                e && (this.pos.x += e), t && (this.pos.y += t), this.render(this.pos.rebound(i, o)), this.emit("crop.change")
            }
        }, {
            key: "createHandles",
            value: function () {
                var e = this;
                return this.options.handles.forEach(function (t) {
                    var n, r = a.default.create("jcrop-handle " + t);
                    r.appendTo(e.el), (0, s.default)(r.el, function () {
                        if (!e.stage.enabled) return !1;
                        var r = e.el.parentElement,
                            i = r.offsetWidth,
                            o = r.offsetHeight;
                        return n = f.default.create(c.default.from(e.el), i, o, t), e.aspect && (n.aspect = e.aspect), e.el.focus(), e.emit("crop.active"), !0
                    }, function (t, r) {
                        return e.render(n.move(t, r))
                    }, function () {
                        e.emit("crop.change")
                    })
                }), this
            }
        }, {
            key: "isActive",
            value: function () {
                return this.stage && this.stage.active === this
            }
        }, {
            key: "render",
            value: function (e) {
                return e = e || this.pos, this.el.style.top = Math.round(e.y) + "px", this.el.style.left = Math.round(e.x) + "px", this.el.style.width = Math.round(e.w) + "px", this.el.style.height = Math.round(e.h) + "px", this.pos = e, this.emit("crop.update"), this
            }
        }, {
            key: "doneDragging",
            value: function () {
                this.pos = c.default.from(this.el)
            }
        }]), t
    }();
    v.create = function () {
        var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {},
            t = document.createElement("div"),
            n = (0, o.default)({}, u.default, e);
        n.uniqueId = Date.now();
        return t.setAttribute("tabindex", "0"), t.setAttribute("uniqueid", n.uniqueId), t.className = n.cropperClass || "jcrop-widget", new (e.widgetConstructor || v)(t, n)
    }, t.default = v
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function (e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }(n(1));
    var i = function (e) {
        function t() {
            return function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, t),
                function (e, t) {
                    if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                    return !t || "object" != typeof t && "function" != typeof t ? e : t
                }(this, (t.__proto__ || Object.getPrototypeOf(t)).apply(this, arguments))
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, r.default), t
    }();
    i.create = function (e) {
        var t = document.createElement("div");
        return t.className = e, new i(t)
    }, t.default = i
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }(),
        i = u(n(0)),
        o = u(n(1)),
        a = u(n(3));

    function u(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }
    var s = function (e) {
        function t(e) {
            var n = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, t);
            var r = function (e, t) {
                if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                return !t || "object" != typeof t && "function" != typeof t ? e : t
            }(this, (t.__proto__ || Object.getPrototypeOf(t)).call(this, e));
            return r.options = {}, Object.defineProperty(r, "_optconf", {
                configurable: !1,
                enumerable: !1,
                value: {},
                writable: !0
            }), r.initOptions(), r.setOptions((0, i.default)({}, a.default, n)), r
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, o.default), r(t, [{
            key: "setOptions",
            value: function (e) {
                var t = this;
                return this.options = (0, i.default)({}, this.options, e), Object.keys(e).forEach(function (n) {
                    t._optconf[n] && t._optconf[n](e[n])
                }), this
            }
        }, {
            key: "initOptions",
            value: function () { }
        }]), t
    }();
    t.default = s
}, function (e, t, n) {
    "use strict";
    var r = e.exports = {
        def: "outQuad",
        swing: function (e, t, n, i) {
            return r[r.def](e, t, n, i)
        },
        inQuad: function (e, t, n, r) {
            return n * (e /= r) * e + t
        },
        outQuad: function (e, t, n, r) {
            return -n * (e /= r) * (e - 2) + t
        },
        inOutQuad: function (e, t, n, r) {
            return (e /= r / 2) < 1 ? n / 2 * e * e + t : -n / 2 * (--e * (e - 2) - 1) + t
        },
        inCubic: function (e, t, n, r) {
            return n * (e /= r) * e * e + t
        },
        outCubic: function (e, t, n, r) {
            return n * ((e = e / r - 1) * e * e + 1) + t
        },
        inOutCubic: function (e, t, n, r) {
            return (e /= r / 2) < 1 ? n / 2 * e * e * e + t : n / 2 * ((e -= 2) * e * e + 2) + t
        },
        inQuart: function (e, t, n, r) {
            return n * (e /= r) * e * e * e + t
        },
        outQuart: function (e, t, n, r) {
            return -n * ((e = e / r - 1) * e * e * e - 1) + t
        },
        inOutQuart: function (e, t, n, r) {
            return (e /= r / 2) < 1 ? n / 2 * e * e * e * e + t : -n / 2 * ((e -= 2) * e * e * e - 2) + t
        },
        inQuint: function (e, t, n, r) {
            return n * (e /= r) * e * e * e * e + t
        },
        outQuint: function (e, t, n, r) {
            return n * ((e = e / r - 1) * e * e * e * e + 1) + t
        },
        inOutQuint: function (e, t, n, r) {
            return (e /= r / 2) < 1 ? n / 2 * e * e * e * e * e + t : n / 2 * ((e -= 2) * e * e * e * e + 2) + t
        },
        inSine: function (e, t, n, r) {
            return -n * Math.cos(e / r * (Math.PI / 2)) + n + t
        },
        outSine: function (e, t, n, r) {
            return n * Math.sin(e / r * (Math.PI / 2)) + t
        },
        inOutSine: function (e, t, n, r) {
            return -n / 2 * (Math.cos(Math.PI * e / r) - 1) + t
        },
        inExpo: function (e, t, n, r) {
            return 0 == e ? t : n * Math.pow(2, 10 * (e / r - 1)) + t
        },
        outExpo: function (e, t, n, r) {
            return e == r ? t + n : n * (1 - Math.pow(2, -10 * e / r)) + t
        },
        inOutExpo: function (e, t, n, r) {
            return 0 == e ? t : e == r ? t + n : (e /= r / 2) < 1 ? n / 2 * Math.pow(2, 10 * (e - 1)) + t : n / 2 * (2 - Math.pow(2, -10 * --e)) + t
        },
        inCirc: function (e, t, n, r) {
            return -n * (Math.sqrt(1 - (e /= r) * e) - 1) + t
        },
        outCirc: function (e, t, n, r) {
            return n * Math.sqrt(1 - (e = e / r - 1) * e) + t
        },
        inOutCirc: function (e, t, n, r) {
            return (e /= r / 2) < 1 ? -n / 2 * (Math.sqrt(1 - e * e) - 1) + t : n / 2 * (Math.sqrt(1 - (e -= 2) * e) + 1) + t
        },
        inElastic: function (e, t, n, r) {
            var i = 1.70158,
                o = 0,
                a = n;
            if (0 == e) return t;
            if (1 == (e /= r)) return t + n;
            if (o || (o = .3 * r), a < Math.abs(n)) {
                a = n;
                i = o / 4
            } else i = o / (2 * Math.PI) * Math.asin(n / a);
            return -a * Math.pow(2, 10 * (e -= 1)) * Math.sin((e * r - i) * (2 * Math.PI) / o) + t
        },
        outElastic: function (e, t, n, r) {
            var i = 1.70158,
                o = 0,
                a = n;
            if (0 == e) return t;
            if (1 == (e /= r)) return t + n;
            if (o || (o = .3 * r), a < Math.abs(n)) {
                a = n;
                i = o / 4
            } else i = o / (2 * Math.PI) * Math.asin(n / a);
            return a * Math.pow(2, -10 * e) * Math.sin((e * r - i) * (2 * Math.PI) / o) + n + t
        },
        inOutElastic: function (e, t, n, r) {
            var i = 1.70158,
                o = 0,
                a = n;
            if (0 == e) return t;
            if (2 == (e /= r / 2)) return t + n;
            if (o || (o = r * (.3 * 1.5)), a < Math.abs(n)) {
                a = n;
                i = o / 4
            } else i = o / (2 * Math.PI) * Math.asin(n / a);
            return e < 1 ? a * Math.pow(2, 10 * (e -= 1)) * Math.sin((e * r - i) * (2 * Math.PI) / o) * -.5 + t : a * Math.pow(2, -10 * (e -= 1)) * Math.sin((e * r - i) * (2 * Math.PI) / o) * .5 + n + t
        },
        inBack: function (e, t, n, r, i) {
            return void 0 == i && (i = 1.70158), n * (e /= r) * e * ((i + 1) * e - i) + t
        },
        outBack: function (e, t, n, r, i) {
            return void 0 == i && (i = 1.70158), n * ((e = e / r - 1) * e * ((i + 1) * e + i) + 1) + t
        },
        inOutBack: function (e, t, n, r, i) {
            return void 0 == i && (i = 1.70158), (e /= r / 2) < 1 ? n / 2 * (e * e * ((1 + (i *= 1.525)) * e - i)) + t : n / 2 * ((e -= 2) * e * ((1 + (i *= 1.525)) * e + i) + 2) + t
        },
        inBounce: function (e, t, n, i) {
            return n - r.outBounce(i - e, 0, n, i) + t
        },
        outBounce: function (e, t, n, r) {
            return (e /= r) < 1 / 2.75 ? n * (7.5625 * e * e) + t : e < 2 / 2.75 ? n * (7.5625 * (e -= 1.5 / 2.75) * e + .75) + t : e < 2.5 / 2.75 ? n * (7.5625 * (e -= 2.25 / 2.75) * e + .9375) + t : n * (7.5625 * (e -= 2.625 / 2.75) * e + .984375) + t
        },
        inOutBounce: function (e, t, n, i) {
            return e < i / 2 ? .5 * r.inBounce(2 * e, 0, n, i) + t : .5 * r.outBounce(2 * e - i, 0, n, i) + .5 * n + t
        }
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }(),
        i = a(n(2)),
        o = a(n(1));

    function a(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }

    function u(e, t) {
        if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
    }
    var s = function () {
        function e(t) {
            u(this, e), "string" == typeof t && (t = document.getElementById(t)), this.el = t, this.shades = {}
        }
        return r(e, [{
            key: "init",
            value: function () {
                var e = this,
                    t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                this.active = void 0 === t.shade || t.shade, this.keys().forEach(function (n) {
                    e.shades[n] = c.create(t, n)
                }), this.el.addEventListener("crop.update", function (t) {
                    t.cropTarget.isActive() && t.cropTarget.options.shade && e.adjust(t.cropTarget.pos)
                }, !1), this.enable()
            }
        }, {
            key: "adjust",
            value: function (e) {
                var t = i.default.from(this.el),
                    n = this.shades;
                n.t.h = e.y, n.b.h = t.h - e.y2, n.t.w = n.b.w = Math.floor(e.w), n.l.w = n.t.x = n.b.x = Math.ceil(e.x), n.r.w = t.w - (Math.ceil(e.x) + Math.floor(e.w))
            }
        }, {
            key: "keys",
            value: function () {
                return ["t", "l", "r", "b"]
            }
        }, {
            key: "enable",
            value: function () {
                var e = this,
                    t = this.shades;
                this.keys().forEach(function (n) {
                    return t[n].insert(e.el)
                })
            }
        }, {
            key: "disable",
            value: function () {
                var e = this.shades;
                this.keys().forEach(function (t) {
                    return e[t].remove()
                })
            }
        }, {
            key: "setStyle",
            value: function (e, t) {
                var n = this.shades;
                this.keys().forEach(function (r) {
                    return n[r].color(e).opacity(t)
                })
            }
        }]), e
    }();
    s.attach = function (e) {
        var t = e.el,
            n = new s(t);
        return n.init(e.options), e.shades = n, e._optconf.shade = function (t) {
            return e.updateShades()
        }, e._optconf.shadeColor = function (e) {
            return n.setStyle(e)
        }, e._optconf.shadeOpacity = function (e) {
            return n.setStyle(null, e)
        }, n
    };
    var c = function (e) {
        function t() {
            return u(this, t),
                function (e, t) {
                    if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                    return !t || "object" != typeof t && "function" != typeof t ? e : t
                }(this, (t.__proto__ || Object.getPrototypeOf(t)).apply(this, arguments))
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, o.default), r(t, [{
            key: "insert",
            value: function (e) {
                e.appendChild(this.el)
            }
        }, {
            key: "remove",
            value: function () {
                this.el.remove()
            }
        }, {
            key: "color",
            value: function (e) {
                return e && (this.el.style.backgroundColor = e), this
            }
        }, {
            key: "opacity",
            value: function (e) {
                return e && (this.el.style.opacity = e), this
            }
        }, {
            key: "w",
            set: function (e) {
                this.el.style.width = e + "px"
            }
        }, {
            key: "h",
            set: function (e) {
                this.el.style.height = e + "px"
            }
        }, {
            key: "x",
            set: function (e) {
                this.el.style.left = e + "px"
            }
        }]), t
    }();
    c.create = function (e, t) {
        var n = document.createElement("div"),
            r = e.shadeClass || "jcrop-shade";
        return n.className = r + " " + t, new c(n).color(e.shadeColor).opacity(e.shadeOpacity)
    }, c.Manager = s, t.default = c
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    }), t.DomObj = t.Shade = t.load = t.easing = t.Sticker = t.Handle = t.Rect = t.Widget = t.Dragger = t.defaults = t.Stage = void 0, t.attach = b;
    var r = y(n(0)),
        i = y(n(3)),
        o = y(n(6)),
        a = y(n(15)),
        u = y(n(7)),
        s = y(n(11)),
        c = y(n(8)),
        f = y(n(4)),
        l = y(n(2)),
        h = y(n(5)),
        d = y(n(1)),
        p = y(n(10)),
        v = y(n(16));

    function y(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }

    function b(e) {
        var t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
        return t = (0, r.default)({}, i.default, t), "string" == typeof e && (e = document.getElementById(e)), "IMG" === e.tagName ? new a.default(e, t) : new o.default(e, t)
    }
    t.Stage = o.default, t.defaults = i.default, t.Dragger = f.default, t.Widget = u.default, t.Rect = l.default, t.Handle = c.default, t.Sticker = h.default, t.easing = p.default, t.load = v.default, t.Shade = s.default, t.DomObj = d.default, t.default = {
        Stage: o.default,
        defaults: i.default,
        Dragger: f.default,
        Widget: u.default,
        Rect: l.default,
        Handle: c.default,
        Sticker: h.default,
        easing: p.default,
        load: v.default,
        attach: b,
        Shade: s.default,
        DomObj: d.default
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }();
    var i = function () {
        function e(t) {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, e), this.widget = t, this.attach()
        }
        return r(e, [{
            key: "attach",
            value: function () {
                var e = this.widget;
                e.el.addEventListener("keydown", function (t) {
                    var n = t.shiftKey ? 10 : 1;
                    switch (t.key) {
                        case "ArrowRight":
                            e.nudge(n);
                            break;
                        case "ArrowLeft":
                            e.nudge(-n);
                            break;
                        case "ArrowUp":
                            e.nudge(0, -n);
                            break;
                        case "ArrowDown":
                            e.nudge(0, n);
                            break;
                        case "Delete":
                        case "Backspace":
                            //e.stage.removeWidget(e);
                            break;
                        default:
                            return
                    }
                    t.preventDefault()
                })
            }
        }]), e
    }();
    i.attach = function (e) {
        return new i(e)
    }, t.default = i
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function (e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }(n(10));
    t.default = function (e, t, n, i) {
        var o = arguments.length > 4 && void 0 !== arguments[4] ? arguments[4] : 30,
            a = arguments.length > 5 && void 0 !== arguments[5] ? arguments[5] : "swing",
            u = ["x", "y", "w", "h"],
            s = t.normalize();
        a = "string" == typeof a ? r.default[a] : a;
        var c = 0;
        return new Promise(function (e, r) {
            requestAnimationFrame(function r() {
                c < o ? (u.forEach(function (e) {
                    s[e] = Math.round(a(c, t[e], n[e] - t[e], o))
                }), i(s), c++, requestAnimationFrame(r)) : (i(n), e())
            })
        })
    }
}, function (e, t, n) {
    "use strict";
    Object.defineProperty(t, "__esModule", {
        value: !0
    });
    var r = function () {
        function e(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                r.enumerable = r.enumerable || !1, r.configurable = !0, "value" in r && (r.writable = !0), Object.defineProperty(e, r.key, r)
            }
        }
        return function (t, n, r) {
            return n && e(t.prototype, n), r && e(t, r), t
        }
    }(),
        i = function (e) {
            return e && e.__esModule ? e : {
                default: e
            }
        }(n(6));
    var o = function (e) {
        function t(e, n) {
            ! function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
            }(this, t);
            var r = function (e) {
                var t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : document.createElement("div");
                return t.className = e, t
            }("jcrop-stage jcrop-image-stage");
            e.parentNode.insertBefore(r, e), r.appendChild(e);
            var i = function (e, t) {
                if (!e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                return !t || "object" != typeof t && "function" != typeof t ? e : t
            }(this, (t.__proto__ || Object.getPrototypeOf(t)).call(this, r, n));
            return i.srcEl = e, e.onload = i.resizeToImage.bind(i), i.resizeToImage(), i
        }
        return function (e, t) {
            if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function, not " + typeof t);
            e.prototype = Object.create(t && t.prototype, {
                constructor: {
                    value: e,
                    enumerable: !1,
                    writable: !0,
                    configurable: !0
                }
            }), t && (Object.setPrototypeOf ? Object.setPrototypeOf(e, t) : e.__proto__ = t)
        }(t, i.default), r(t, [{
            key: "resizeToImage",
            value: function () {
                var e = this.srcEl.width,
                    t = this.srcEl.height;
                this.el.style.width = e + "px", this.el.style.height = t + "px", this.refresh()
            }
        }, {
            key: "destroy",
            value: function () {
                this.el.parentNode.insertBefore(this.srcEl, this.el), this.el.remove()
            }
        }]), t
    }();
    t.default = o
}, function (e, t, n) {
    "use strict";

    function r(e) {
        return "string" == typeof e && (e = document.getElementById(e)), new Promise(function (t, n) {
            if (r.check(e)) return t(e);

            function i(r) {
                e.removeEventListener("load", i), e.removeEventListener("error", i), "load" === r.type ? t(e) : n(e)
            }
            e.addEventListener("load", i), e.addEventListener("error", i)
        })
    }
    Object.defineProperty(t, "__esModule", {
        value: !0
    }), r.check = function (e) {
        return !!e.complete && 0 !== e.naturalWidth
    }, t.default = r
}]);
//# sourceMappingURL=jcrop.js.map
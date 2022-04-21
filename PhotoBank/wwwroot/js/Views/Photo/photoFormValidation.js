var app = Vue.createApp({
    data() {
        return {
            errorList: []
        }
    },
    methods: {
        checkPhotoName() {
            var err = this.errorList.find(errorList => errorList.name === 'errorPhotoName');
            if (err!= undefined)
                this.errorList.pop(err);
            var value = this.$refs.photoName.value;
            if (value.trim().length === 0) {
                this.errorList.push({ name: "errorPhotoName", message: "Photo name can't be empty" });
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorPhotoName'));
            }
        },
        checkPhotoDescription() {
            var err = this.errorList.find(errorList => errorList.name === 'errorPhotoDescription');
            if (err!= undefined)
                this.errorList.pop(err);
            var value = this.$refs.photoDescription.value;
            if (value.trim().length === 0) {
                this.errorList.push({ name: "errorPhotoDescription", message: "Photo description can't be empty" });
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorPhotoDescription'));
            }
        },
        checkPhotoPrice() {
            var err = this.errorList.find(errorList => errorList.name === 'errorPhotoPrice');
            if (err!= undefined)
                this.errorList.pop(err);
            var value = this.$refs.photoPrice.value;
            if (Number(value) === 0) {
                this.errorList.push({ name: "errorPhotoPrice", message: "Photo price must be grater then 0" });
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorPhotoPrice'));
            }
        },
        checkFileName() {
            var err = this.errorList.find(errorList => errorList.name === 'errorFileName');
            if (err!= undefined)
                this.errorList.pop(err);
            var value = this.$refs.photoFileName.value;
            if (value.trim().length === 0) {
                this.errorList.push({ name: "errorFileName", message: "Photo file can't be empty" });
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorFileName'));
                var img = this.$refs.image;
                var input = document.getElementById("photoFileName");
                var fReader = new FileReader();
                fReader.readAsDataURL(input.files[0]);
                fReader.onloadend = function (event) {
                        img.src = event.target.result;
                }
            }
        },

        validateForm() {
            this.errorList.splice(0, this.errorList.length);
            this.checkFileName();
            this.checkPhotoName();
            this.checkPhotoDescription();
            this.checkPhotoPrice();
            if (this.errorList.length > 0) {
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
        }
    },
    mounted() {
        this.$refs.photoName.focus();
    }
});

app.mount('#inputForm');
var app = Vue.createApp({
    data() {
        return{
            errorList: [] 
        }
    },
    methods: {
        checkCategoryName() {
            var categoryName = this.$refs.categoryName.value;
            if (categoryName.trim().length === 0) {
                this.errorList.push({ name: "errorCategoryName", message: "Category name can't be empty" });
            } else {
                this.errorList.pop(this.errorList.find(errorList => errorList.name === 'errorCategoryName'));
                
            }
        },
        validateForm() {
            this.errorList.splice(0, this.errorList.length);
            this.checkCategoryName();
            if (this.errorList.length>0) {
            this.$refs.submit.addEventListener('submit',
                event => {
                    event.preventDefault();
                });
            }
        }
    }
});

app.mount('#inputForm');